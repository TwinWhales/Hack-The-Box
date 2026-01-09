import os
from itertools import cycle

# --- 설정값 ---

# 1. XOR 키 재구성 (PowerShell $k 변수에서 추출 및 정리)
# 난독화된 문자열을 연결하여 실제 키를 생성합니다.
key_parts = [
    '6i', 'I', 'oN', 'o', 'Mk5', 'iRYAw', '7Z', 'TWed0Cr',
    'juZ9wijyQDj', 'Py', '9Ms0D8K0Z2H5MX6wyOKqFxl', 'Om1', 'G',
    'pjmYfaQX', 'acA6'
]
xor_key = "".join(key_parts).encode('utf-8')

# 2. 파일 이름 설정
INPUT_FILE = 'vm.html'
OUTPUT_FILE = 'www4-decrypted.dll'

# --- 파일 처리 및 복호화 로직 ---

try:
    # 1. 암호화된 데이터 (vm.html) 읽기
    with open(INPUT_FILE, 'rb') as f:
        enc_data = f.read()

    data_length = len(enc_data)
    if data_length == 0:
        print(f"오류: {INPUT_FILE} 파일이 비어 있습니다.")
    else:
        # 2. 복호화 연산 수행
        # PowerShell의 $b[$i] -bxor $k[$i%$k.length] 로직을 구현합니다.
        zipped_bytes = zip(enc_data, cycle(xor_key))
        decrypted_bytes = bytearray([enc_byte ^ key_byte for enc_byte, key_byte in zipped_bytes])

        # 3. 복호화된 데이터 출력 파일에 쓰기
        with open(OUTPUT_FILE, 'wb') as f:
            f.write(decrypted_bytes)

        # 4. 결과 출력
        print("=" * 40)
        print(f"✅ 복호화 성공: {INPUT_FILE} ({data_length} bytes)")
        print(f"➡️ 결과 파일 저장: {OUTPUT_FILE}")
        print("-" * 40)
        
        # 첫 100바이트 출력 (DLL 헤더 확인용)
        print("--- Decrypted Hex Dump (First 100 bytes) ---")
        
        # MZ\x90\x00... 형태로 시작하는지 확인 (Windows 실행 파일/DLL 헤더)
        hex_dump = ' '.join(f'{b:02x}' for b in decrypted_bytes[:100])
        print(decrypted_bytes[:100])
        
        print("-" * 40)
        print(f"참고: 파일 크기는 {data_length} 바이트입니다.")
        
except FileNotFoundError:
    print(f"❌ 오류: 지정된 위치에 {INPUT_FILE} 파일을 찾을 수 없습니다.")
except Exception as e:
    print(f"❌ 복호화 중 오류 발생: {e}")