# decrypt_safe.ps1

# 스크립트가 실행될 때 2개의 인자를 받도록 설정합니다.
param (
    # [필수] 복호화할 대상 파일 (예: csrss.exe)
    [Parameter(Mandatory=$true)]
    [string]$FilePath,

    # [필수] 키로 사용할 파일 (예: csrss.dll)
    [Parameter(Mandatory=$true)]
    [string]$KeyPath
)

Write-Host "--- 안전한 복호화 스크립트 실행 ---"

# 1. 키 파일과 암호화된 파일을 바이트 배열로 읽어옵니다.
try {
    Write-Host "[1] 키 파일 읽는 중: $KeyPath"
    $key = [System.IO.File]::ReadAllBytes($KeyPath)
    
    Write-Host "[2] 대상 파일 읽는 중: $FilePath"
    $fileContent = [System.IO.File]::ReadAllBytes($FilePath)
}
catch {
    Write-Error "오류: 파일을 읽지 못했습니다. 경로를 확인하세요."
    Write-Error $_
    return
}

# 2. 키 파일의 길이를 저장합니다.
$keyLength = $key.Length
Write-Host "[3] 키 길이 확인: ${keyLength} 바이트"

# 3. 암호화된 파일의 처음부터 끝까지 루프를 돕니다.
Write-Host "[4] XOR 복호화 연산 시작..."
for ($i = 0; $i -lt $fileContent.Length; $i++) {
    # 4. 파일의 각 바이트를 키의 바이트와 반복 XOR(-bxor) 연산합니다.
    $fileContent[$i] = $fileContent[$i] -bxor $key[$i % $keyLength]
}
Write-Host "[5] 연산 완료."

# 5. [가장 중요] 복호화된 내용을 *새 파일*로 저장합니다.
$newFilePath = $FilePath + ".decrypted.bin"
try {
    [System.IO.File]::WriteAllBytes($newFilePath, $fileContent)
    Write-Host "[성공] 복호화된 결과가 다음 파일로 저장되었습니다:"
    Write-Host "==> $newFilePath"
}
catch {
    Write-Error "오류: 결과 파일을 저장하지 못했습니다."
    Write-Error $_
}

Write-Host "--- 스크립트 종료 ---"