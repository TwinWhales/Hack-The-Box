# Configuration
$maxSizeMB = 50
# 1. 확장자 블랙리스트 (기존)
$globalExtensions = @(".zip", ".7z", ".rar", ".tar", ".gz", ".ad1", ".E01", ".iso", ".vmem", ".vmdk", ".vdi", ".pcap", ".pcapng", ".cap", ".exe", ".dll", ".so", ".bin", ".log", ".tmp")
# 2. [NEW] 폴더 블랙리스트 (이 이름이 포함된 경로는 무조건 무시)
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

Write-Host ">>> Starting Smart Auto-Upload (Targeted Mode + Folder Filter)..." -ForegroundColor Cyan

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
    
    # [NEW] 폴더 블랙리스트 체크 (가상환경, 라이브러리 폴더 등)
    # 경로 중간에 블랙리스트 폴더명이 껴있는지 확인
    $folderMatch = $null
    foreach ($part in $pathParts) {
        if ($globalIgnoreFolders -contains $part) {
            $folderMatch = $part
            break
        }
    }

    if ($folderMatch) {
        # 예: Challenges/Forensic/Easy/POOF/venv_decrypt/bin/python
        # -> venv_decrypt 폴더 자체를 ignore 목록에 추가하기 위해 경로 계산
        # 여기서는 단순히 해당 파일 경로를 추가하면 gitignore 업데이트 로직에서 처리됨
        # 하지만 더 깔끔하게 하기 위해 해당 폴더 루트를 찾을 수도 있음.
        # 편의상 파일 경로를 그대로 넘기면, 아래 gitignore 로직이 그 파일을 무시함.
        # 더 완벽하게 하려면 "폴더/"를 추가해야 하지만, 파일별로 막아도 충분함.
        # 또는 사용자가 지정한 'venv_decrypt'가 포함된 경로라면 해당 폴더 자체를 ignore 하는게 좋음.
        
        # 로직 개선: 블랙리스트에 걸린 폴더 경로 자체를 찾아서 추가
        # 예: .../venv_decrypt/... 라면 .../venv_decrypt/ 를 추가
        
        $ignorePath = $relativePath
        # 경로 중 블랙리스트 폴더까지만 잘라냄 (정확한 폴더 ignore를 위함)
        $splitIndex = $pathParts.IndexOf($folderMatch)
        if ($splitIndex -ge 0) {
            # 부분 경로 재조립
            $subPathParts = $pathParts[0..$splitIndex]
            $ignorePath = [String]::Join("/", $subPathParts) + "/" # 끝에 슬래시 붙여서 폴더임을 명시
        }

        if (-not $filesToIgnore.Contains($ignorePath)) {
            Write-Host " [IGNORE] Blocked Folder Found: $ignorePath" -ForegroundColor Red
            $filesToIgnore.Add($ignorePath)
        }
        continue # 이 파일은 더 이상 검사 안 함
    }

    # 파일 검사 시작
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
# 3. Targeted README Update
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
                 $filesList += "- **$relativePath** ($sizeStr)"
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
# 4. Git Push with Smart Commit Message & Date
# ---------------------------------------------------------
Write-Host "`n----------------------------------------"
Write-Host "4. Preparing Git Push..."
Start-Sleep -Seconds 1

$status = git status --short

if ($status) {
    Write-Host "Detected changes:" -ForegroundColor Cyan
    $status | ForEach-Object { Write-Host "   $_" }
    
    $todayDate = Get-Date -Format "yyyy-MM-dd"
    $fullTimestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    
    $commitSubject = "Auto upload ($todayDate)"
    $commitBody = "Timestamp: $fullTimestamp"
    
    if ($affectedRoots.Count -gt 0) {
        $problemNames = $affectedRoots | ForEach-Object { Split-Path $_ -Leaf } | Select-Object -Unique
        $nameStr = $problemNames -join ", "
        if ($nameStr.Length -gt 50) { $nameStr = $nameStr.Substring(0, 47) + "..." }
        
        $commitSubject = "Add/Update: $nameStr ($todayDate)"
        $commitBody += "`n`nUpdated Problems:`n" + ($problemNames -join "`n")
    } elseif ($status -match "\.gitignore") {
        $commitSubject = "Config: Update .gitignore ($todayDate)"
    }

    Write-Host "`nGenerated Commit Message:" -ForegroundColor Magenta
    Write-Host "Subject: $commitSubject"
    
    $confirm = Read-Host "`nDo you want to commit and push? (y/n)"
    if ($confirm -eq 'y') {
        Write-Host " -> Staging files (git add -A)..."
        
        # [NEW] 에러가 났던 venv 같은게 있으면 git add에서 또 터질 수 있으니
        # gitignore가 먼저 적용되었는지 확인하기 위해 순차 실행
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