
import base64
import os

def solve(data):
    # 1. 쓰레기 문자열 "][(s)]w" 제거
    cleaned = data.replace("][(s)]w", "").replace("\n", "")
    
    if not cleaned:
        print("[!] Cleaning resulted in empty string.")
        return

    # 2. VBA 로직 구현
    # - 앞의 50글자는 그대로 둠 (PowerShell 헤더 부분)
    header = cleaned[:50]
    
    # - 51번째 글자(인덱스 50)부터 끝까지 2칸씩 건너뛰며 가져옴 (Step 2)
    payload_part = cleaned[50::2]
    
    # 3. 합치기
    full_command = header + payload_part
    print(f"[-] 1차 복구된 명령어 (앞부분): {full_command[:100]}...") 

    # 4. Base64 디코딩 (PowerShell EncodedCommand는 주로 UTF-16LE로 인코딩됨)
    try:
        # "-ENCOD " 뒷부분만 잘라내기 (공백 개수에 따라 split 조정 필요할 수 있음)
        # 보통 -encoded command 뒤에 공백이 좀 있으니 안전하게 "encoded" 뒤쪽을 찾음
        # cleaned string might vary in casing? "ENCOD" matches the user's snippet.
        
        # Search for ENCOD pattern roughly
        if "ENCOD" in full_command:
            # Split and take the last part.
            # Warning: checks for multiple occurrences? simplified approach:
            b64_str_candidate = full_command.split("ENCOD")[1]
            
            # The header usually ends with something like ' -windowstyle hidden -ENCOD '
            # So the b64 string is likely immediately after possibly some spaces.
            b64_str = b64_str_candidate.strip()
            
            # Remove leading "ED " or similar if split wasn't perfect, matching user hint
            # User said: if b64_str.startswith("ED"): b64_str = b64_str[2:].strip()
            # But "ENCOD" split consumes 'ENCOD'. If proper flags were used, it might be '-ENCODED '.
            # Let's see what remains.
            # If the original was "... -ENCODED ...", split("ENCOD") leaves "ED ..."
            if b64_str.startswith("ED"):
                b64_str = b64_str[2:].strip()
            
            # Clean up potential garbage at the end? Base64 usually ends with =
            # But simple decoding might ignore garbage or throw error.
            
            print(f"[-] Base64 Candidate (First 50): {b64_str[:50]}...")
            
            # Base64 디코딩
            decoded_bytes = base64.b64decode(b64_str)
            # PowerShell은 UTF-16LE를 씁니다
            decoded_script = decoded_bytes.decode('utf-16le', errors='ignore')
            
            print("\n[+] ★★★ 최종 복호화된 PowerShell 스크립트 ★★★")
            print("="*50)
            print(decoded_script)
            print("="*50)
            
            # Save to file for convenience
            with open('final_script.ps1', 'w', encoding='utf-8') as f:
                f.write(decoded_script)
            print("[+] saved to final_script.ps1")
            
            return decoded_script
        else:
            print("[!] 'ENCOD' not found in full command.")
            print(full_command[:200])
            
    except Exception as e:
        print(f"\n[!] Base64 디코딩 중 오류 발생: {e}")
        # print("1차 복구된 명령어에서 직접 Base64 부분을 복사해서 디코딩해보세요.")

# Read from strings_result.txt
filepath = 'c:\\Users\\user\\Desktop\\Hack The Box\\Challenges\\Forensic\\Easy\\emo\\strings_result.txt'
if os.path.exists(filepath):
    found_data = ""
    with open(filepath, 'r', encoding='latin1') as f: # strings output might be any encoding, latin1 holds bytes
         # Line 3 seemed to have the data.
         # But let's search for the pattern "][(s)]w"
         for line in f:
             if "][(s)]w" in line:
                 # Found the line with obfuscation
                 found_data = line
                 break
    
    if found_data:
        solve(found_data)
    else:
        print("Pattern '][(s)]w' not found in strings_result.txt")
else:
    print(f"File not found: {filepath}")
