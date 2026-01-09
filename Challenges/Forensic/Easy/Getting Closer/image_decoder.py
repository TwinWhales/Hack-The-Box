import base64
import re

# 입력 파일명
input_file = "image.jpg"
output_file = "extracted_payload.txt"

# 파일 읽기 (텍스트 모드로 열면 안 됨)
with open(input_file, "rb") as f:
    data = f.read()

# 바이트 데이터를 문자열로 변환 (Latin-1을 사용하면 손상 없이 가능)
text = data.decode("latin1")

# Base64 마커 사이 문자열 추출
match = re.search(r"<<BASE64_START>>(.*?)<<BASE64_END>>", text, re.DOTALL)

if match:
    base64_data = match.group(1).strip()
    try:
        decoded = base64.b64decode(base64_data)
        with open(output_file, "wb") as out:
            out.write(decoded)
        print(f"[+] Base64 payload extracted and saved to: {output_file}")
    except Exception as e:
        print(f"[!] Failed to decode base64: {e}")
else:
    print("[!] Base64 markers not found in the image.")
