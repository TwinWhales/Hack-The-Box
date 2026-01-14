# Configuration
$maxSizeMB = 50
$TargetRepo = "https://github.com/TwinWhales/Hack-The-Box.git"
# 1. 확장자 블랙리스트
$globalExtensions = @(".zip", ".7z", ".rar", ".tar", ".gz", ".ad1", ".E01", ".iso", ".vmem", ".vmdk", ".vdi", ".pcap", ".pcapng", ".cap", ".exe", ".dll", ".so", ".bin", ".log", ".tmp")
# 2. 폴더 블랙리스트
$globalIgnoreFolders = @("venv", ".venv", "env", "venv_decrypt", "__pycache__", "node_modules", ".git", ".idea", ".vscode")

$root = Get-Location
$gitignorePath = Join-Path $root ".gitignore"
$markerStart = ""
$markerEnd = ""

# Function to get Problem Root
function Get-ProblemRoot($fullPath) {
    if (-not $fullPath.StartsWith($root.Path)) { return $null }
    $relativePath = $fullPath.Substring($root.Path.Length + 1)
    $pathParts = $relativePath.Split([System.IO.Path]::DirectorySeparatorChar)
    
    if ($pathParts.Count -eq 0) { return $null }
    $category = $pathParts[0]

    if ($category -eq "Machines" -and $pathParts.Count -ge 2) {
        return Join-Path $root (Join-Path $pathParts[0] $pathParts[1])
    }
    elseif ($category -eq "Sherlocks" -and $pathParts.Count -ge 3) {
        return Join-Path $root (Join-Path $pathParts[0] (Join-Path $pathParts[1] $pathParts[2]))
    }
    elseif ($category -eq "Challenges" -and $pathParts.Count -ge 4) {
        return Join-Path $root (Join-Path $pathParts[0] (Join-Path $pathParts[1] (Join-Path $pathParts[2] $pathParts[3])))
    }
    return $null
}

# ---------------------------------------------------------
# 0. Remote Repository Verification & Auto-Fix
# ---------------------------------------------------------
Write-Host "0. Verifying Remote Repository..."

$currentRemote = git remote get-url origin 2>$null

if (-not $currentRemote) {
    Write-Host " -> No remote configured. Setting origin to: $TargetRepo" -ForegroundColor Yellow
    git remote add origin $TargetRepo
} elseif ($currentRemote.Trim() -ne $TargetRepo) {
    Write-Host " -> Current remote is '$currentRemote'" -ForegroundColor Red
    Write-Host " -> Target remote is  '$TargetRepo'" -ForegroundColor Green
    
    $fixRemote = Read-Host "Remote URL mismatch. Do you want to switch to the target repo? (y/n)"
    if ($fixRemote -eq 'y') {
        git remote set-url origin $TargetRepo
        Write-Host " -> Remote switched to $TargetRepo" -ForegroundColor Cyan
    } else {
        Write-Host " -> Exiting script to prevent upload to wrong repo." -ForegroundColor Red
        exit
    }
} else {
    Write-Host " -> Remote is correctly set to: $TargetRepo" -ForegroundColor Green
}

Write-Host ">>> Starting Smart Auto-Upload (Targeted Mode + Extracted Check)..." -ForegroundColor Cyan

# ---------------------------------------------------------
# 1. Identify New Untracked Files & Filters
# ---------------------------------------------------------
Write-Host "1. Analyzing git status for new files..."

$untrackedFiles = git ls-files --others --exclude-standard
$filesToIgnore = [System.Collections.Generic.List[string]]::new()
$affectedRoots = [System.Collections.Generic.HashSet[string]]::new()

foreach ($relativePath in $untrackedFiles) {
    $fullPath = Join-Path $root $relativePath
    $pathParts = $relativePath.Split([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
    
    # 폴더 블랙리스트 체크
    $folderMatch = $null
    foreach ($part in $pathParts) {
        if ($globalIgnoreFolders -contains $part) {
            $folderMatch = $part
            break
        }
    }

    if ($folderMatch) {
        $ignorePath = $relativePath
        $splitIndex = $pathParts.IndexOf($folderMatch)
        if ($splitIndex -ge 0) {
            $subPathParts = $pathParts[0..$splitIndex]
            $ignorePath = [String]::Join("/", $subPathParts) + "/" 
        }

        if (-not $filesToIgnore.Contains($ignorePath)) {
            Write-Host " [IGNORE] Blocked Folder Found: $ignorePath" -ForegroundColor Red
            $filesToIgnore.Add($ignorePath)
        }
        continue 
    }

    # 파일 검사
    $item = $null
    $pathExists = $false
    try {
        $pathExists = Test-Path -LiteralPath $fullPath -PathType Leaf -ErrorAction SilentlyContinue
    } catch { $pathExists = $false }

    if ($pathExists) {
        try {
            $item = Get-Item -LiteralPath $fullPath -ErrorAction Stop
        } catch {
            try {
                $longPath = "\\?\$fullPath"
                $item = Get-Item -LiteralPath $longPath -ErrorAction Stop
            } catch { continue }
        }
    } else {
        try {
            $longPath = "\\?\$fullPath"
            $item = Get-Item -LiteralPath $longPath -ErrorAction SilentlyContinue
        } catch { continue }
    }

    if ($item) {
        $sizeMB = $item.Length / 1MB
        $shouldIgnore = $false

        if ($sizeMB -gt $maxSizeMB) {
            Write-Host " [TARGET] Large file: $relativePath ($("{0:N2} MB" -f $sizeMB))" -ForegroundColor Yellow
            $shouldIgnore = $true
        }
        elseif ($globalExtensions -contains $item.Extension) {
            Write-Host " [TARGET] Blocked extension: $relativePath" -ForegroundColor Yellow
            $shouldIgnore = $true
        }

        if ($shouldIgnore) {
            $filesToIgnore.Add($relativePath)
            $pRoot = Get-ProblemRoot $item.FullName
            if ($pRoot) { [void]$affectedRoots.Add($pRoot) }
        }
    }
}

if ($filesToIgnore.Count -eq 0) {
    Write-Host "No new files to exclude. Everything looks clean." -ForegroundColor Green
}

# ---------------------------------------------------------
# 2. Update .gitignore
# ---------------------------------------------------------
if ($filesToIgnore.Count -gt 0) {
    Write-Host "2. Updating .gitignore..."
    $content = ""
    if (Test-Path $gitignorePath) {
        $content = Get-Content $gitignorePath -Raw -Encoding UTF8
    }
    if (-not $content.EndsWith("`n")) { $content += "`n" }
    
    $addedCount = 0
    foreach ($file in $filesToIgnore) {
        $gitPath = $file -replace "\\", "/"
        if ($content -notmatch "(?m)^" + [regex]::Escape($gitPath) + "$") {
            $content += "$gitPath`n"
            $addedCount++
            Write-Host "   + Added: $gitPath" -ForegroundColor Cyan
        }
    }
    
    if ($addedCount -gt 0) {
        Set-Content -Path $gitignorePath -Value $content -Encoding UTF8
    }
}

# ---------------------------------------------------------
# 3. Targeted README Update (With Extraction Check)
# ---------------------------------------------------------
if ($affectedRoots.Count -gt 0) {
    Write-Host "3. Updating READMEs for affected folders only..."
    
    foreach ($dir in $affectedRoots) {
        $folderName = Split-Path $dir -Leaf
        Write-Host " -> Processing folder: $folderName" -ForegroundColor Magenta

        $candidates = Get-ChildItem -Path $dir -Recurse -File -ErrorAction SilentlyContinue | Where-Object { 
            ($globalExtensions -contains $_.Extension) -or ($_.Length / 1MB -gt $maxSizeMB) 
        }

        if (-not $candidates) { continue }

        $relPaths = $candidates | ForEach-Object { $_.FullName.Substring($root.Path.Length + 1).Replace("\", "/") }
        $ignoredRelPaths = $relPaths | git check-ignore --stdin 2>$null

        if (-not $ignoredRelPaths) { continue }

        $filesList = @()
        foreach ($ignoredPath in $ignoredRelPaths) {
            $normIgnored = $ignoredPath -replace "/", "\"
            $match = $candidates | Where-Object { $_.FullName.EndsWith($normIgnored) } | Select-Object -First 1
            
            if ($match) {
                 $relativePath = $match.FullName.Substring($dir.Length + 1)
                 $size = $match.Length / 1MB
                 $sizeStr = "{0:N2} MB" -f $size
                 
                 $extraNote = ""
                 if ($relativePath -match "(\.extracted|_extracted|squashfs-root|jffs2-root)") {
                     $extraNote = " <br> *(Extracted/Incompatible content - Not uploaded)*"
                 }

                 $filesList += "- **$relativePath** ($sizeStr)$extraNote"
            }
        }

        if ($filesList.Count -eq 0) { continue }

        $readmePath = Join-Path $dir "README.md"
        $listContent = $filesList -join "`r`n"
        $managedBlock = "$markerStart`r`n## Excluded Files`r`nThe following files exist in this folder but were excluded from the repository due to file size limits or file type restrictions:`r`n`r`n$listContent`r`n$markerEnd"

        if (Test-Path $readmePath) {
            $original = Get-Content $readmePath -Raw -Encoding UTF8
            $escapedStart = [regex]::Escape($markerStart)
            $escapedEnd = [regex]::Escape($markerEnd)

            if ($original -match "$escapedStart(?s).*$escapedEnd") {
                $newContent = $original -replace "$escapedStart(?s).*$escapedEnd", $managedBlock
                if ($newContent -ne $original) {
                    Set-Content -Path $readmePath -Value $newContent -Encoding UTF8
                    Write-Host "    Updated README (Content changed)"
                }
            } elseif ($original -match "## Excluded Files") {
                 $newContent = $original -replace "(?s)## Excluded Files.*", $managedBlock
                 Set-Content -Path $readmePath -Value $newContent -Encoding UTF8
                 Write-Host "    Migrated README to Safe Format"
            } else {
                $newContent = $original + "`r`n`r`n" + $managedBlock
                Set-Content -Path $readmePath -Value $newContent -Encoding UTF8
                Write-Host "    Appended to README"
            }
        } else {
            $header = "# $folderName"
            $newContent = $header + "`r`n`r`n" + $managedBlock
            Set-Content -Path $readmePath -Value $newContent -Encoding UTF8
            Write-Host "    Created New README"
        }
    }
} else {
    Write-Host "3. No folders require README updates."
}

# ---------------------------------------------------------
# 4. Git Push with Problem Name in Commit Message
# ---------------------------------------------------------
Write-Host "`n----------------------------------------"
Write-Host "4. Preparing Git Push..."
Start-Sleep -Seconds 1

# Git 상태 확인
$statusOutput = git status --short

if ($statusOutput) {
    Write-Host "Detected changes:" -ForegroundColor Cyan
    $statusOutput | ForEach-Object { Write-Host "   $_" }
    
    $todayDate = Get-Date -Format "yyyy-MM-dd"
    $fullTimestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    
    # [NEW] 변경된 파일들을 분석하여 문제 폴더 이름을 추출
    $changedProblemNames = [System.Collections.Generic.HashSet[string]]::new()
    
    foreach ($line in $statusOutput) {
        # git status output 예: " M Challenges/Web/SomeProblem/file.txt"
        # 앞의 상태 코드(M, ??, A 등)와 공백을 제거하고 경로만 추출
        if ($line.Length -gt 3) {
            $relPath = $line.Substring(3).Trim()
            $fullPath = Join-Path $root $relPath
            
            # 경로를 기반으로 문제 루트 찾기
            $pRoot = Get-ProblemRoot $fullPath
            if ($pRoot) {
                $pName = Split-Path $pRoot -Leaf
                [void]$changedProblemNames.Add($pName)
            }
        }
    }

    # 커밋 메시지 생성 로직
    if ($changedProblemNames.Count -gt 0) {
        # 문제 이름이 여러 개일 경우 콤마로 연결
        $nameStr = $changedProblemNames -join ", "
        # 너무 길면 자르기 (Git 제목 길이 제한 고려)
        if ($nameStr.Length -gt 50) { $nameStr = $nameStr.Substring(0, 47) + "..." }
        
        # 요청하신 포맷: "문제명 - 날짜"
        $commitSubject = "$nameStr - $todayDate"
        $commitBody = "Timestamp: $fullTimestamp`n`nUpdated Problems:`n" + ($changedProblemNames -join "`n")
    } else {
        # 문제 폴더가 아닌 루트 파일(.gitignore 등)만 변경된 경우
        if ($statusOutput -match "\.gitignore") {
            $commitSubject = "Config: Update .gitignore - $todayDate"
        } else {
            $commitSubject = "Auto upload - $todayDate"
        }
        $commitBody = "Timestamp: $fullTimestamp"
    }

    Write-Host "`nGenerated Commit Message:" -ForegroundColor Magenta
    Write-Host "Subject: $commitSubject"
    
    $confirm = Read-Host "`nDo you want to commit and push? (y/n)"
    if ($confirm -eq 'y') {
        Write-Host " -> Staging files (git add -A)..."
        
        git add .gitignore
        git add -A
        
        $staged = git diff --name-only --cached
        if (-not $staged) {
            Write-Host "Error: 'git add' failed. No files staged." -ForegroundColor Red
            Write-Host "Possible Reason: Invalid files (like venv symlinks) might still be blocking git." -ForegroundColor Yellow
        } else {
            git commit -m "$commitSubject" -m "$commitBody"
            Write-Host " -> Pushing to remote..."
            git push origin main
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host ">>> Done! Successfully pushed." -ForegroundColor Green
            } else {
                Write-Host ">>> Push failed." -ForegroundColor Red
            }
        }
    } else {
        Write-Host ">>> Cancelled." -ForegroundColor Yellow
    }
} else {
    Write-Host "Nothing to commit. Working tree clean." -ForegroundColor Green
}