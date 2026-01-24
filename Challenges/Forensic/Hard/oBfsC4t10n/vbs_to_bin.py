import re

def parse_vbs_array(vbs_file, output_bin):
    try:
        with open(vbs_file, 'r', encoding='utf-8') as f:
            content = f.read()
            
        # "Array(&HDD, &HC1, ...)" 패턴을 찾습니다.
        match = re.search(r'Array\((.*?)\)', content, re.DOTALL)
        if not match:
            print("[-] Array를 찾을 수 없습니다.")
            return

        # 쉼표로 나누고 &H 제거 후 바이트로 변환
        hex_str = match.group(1).replace('&H', '').replace(' ', '').replace('\n', '').replace('_', '')
        hex_list = hex_str.split(',')
        
        byte_data = bytearray()
        for h in hex_list:
            if h: # 빈 문자열 제외
                byte_data.append(int(h, 16))

        with open(output_bin, 'wb') as f:
            f.write(byte_data)
            
        print(f"[+] 성공! '{output_bin}' 생성 완료 ({len(byte_data)} bytes).")
        print("[+] 이제 아래 명령어로 분석하세요:")
        print(f"    scdbg -f {output_bin} -s -1")

    except Exception as e:
        print(f"[-] 오류: {e}")

# 실행 (아까 만든 vbs 파일 이름)
if __name__ == "__main__":
    parse_vbs_array("payload_extracted.hta", "final_shellcode.bin")