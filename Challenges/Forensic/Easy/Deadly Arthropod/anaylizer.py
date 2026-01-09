import csv
import sys

# --- 사용자 설정 ---

# 1. 분석할 CSV 파일 경로
#    (윈도우 경로인 경우: r'C:\Users\user\captures.csv' 처럼 r을 붙여주세요)
CSV_FILE = r'C:\Users\user\Desktop\Pentest\Forensic\Deadly Arthropod\captures.csv'

# 2. CSV 파일에서 HID 데이터가 포함된 헤더(열) 이름
#    (사용자님이 주신 예제 기준 'HID Data'가 맞습니다)
DATA_COLUMN_NAME = 'HID Data'

# 3. 결과를 저장할 파일 이름
OUTPUT_FILE = 'output_keys.txt'

# --- 설정 끝 ---


# 표준 USB HID 스캔 코드 맵 (QWERTY 기준)
SCANCODE_MAP = {
    0x04: ('a', 'A'), 0x05: ('b', 'B'), 0x06: ('c', 'C'), 0x07: ('d', 'D'),
    0x08: ('e', 'E'), 0x09: ('f', 'F'), 0x0A: ('g', 'G'), 0x0B: ('h', 'H'),
    0x0C: ('i', 'I'), 0x0D: ('j', 'J'), 0x0E: ('k', 'K'), 0x0F: ('l', 'L'),
    0x10: ('m', 'M'), 0x11: ('n', 'N'), 0x12: ('o', 'O'), 0x13: ('p', 'P'),
    0x14: ('q', 'Q'), 0x15: ('r', 'R'), 0x16: ('s', 'S'), 0x17: ('t', 'T'),
    0x18: ('u', 'U'), 0x19: ('v', 'V'), 0x1A: ('w', 'W'), 0x1B: ('x', 'X'),
    0x1C: ('y', 'Y'), 0x1D: ('z', 'Z'),
    0x1E: ('1', '!'), 0x1F: ('2', '@'), 0x20: ('3', '#'), 0x21: ('4', '$'),
    0x22: ('5', '%'), 0x23: ('6', '^'), 0x24: ('7', '&'), 0x25: ('8', '*'),
    0x26: ('9', '('), 0x27: ('0', ')'),
    0x28: ('\n', '\n'),       # Enter
    0x29: ('[ESC]', '[ESC]'), # Escape
    0x2A: ('[BKSP]', '[BKSP]'), # Backspace
    0x2B: ('\t', '\t'),       # Tab
    0x2C: (' ', ' '),       # Space
    0x2D: ('-', '_'), 0x2E: ('=', '+'), 0x2F: ('[', '{'), 0x30: (']', '}'),
    0x31: ('\\', '|'), 0x33: (';', ':'), 0x34: ("'", '"'), 0x35: ('`', '~'),
    0x36: (',', '<'), 0x37: ('.', '>'), 0x38: ('/', '?'),
    0x39: ('[CAPS]', '[CAPS]'), # Caps Lock
    0x4F: ('[RIGHT]', '[RIGHT]'), # Right Arrow
    0x50: ('[LEFT]', '[LEFT]'),   # Left Arrow
    0x51: ('[DOWN]', '[DOWN]'),   # Down Arrow
    0x52: ('[UP]', '[UP]'),     # Up Arrow
}


def main():
    """
    CSV 파일을 DictReader로 읽어 지정된 열의 16진수 문자열을 분석합니다.
    """
    result_chars = []
    pressed_keys = set() 

    try:
        # CSV 파일 열기 (encoding은 cp949 또는 utf-8-sig 등을 시도)
        with open(CSV_FILE, 'r', encoding='utf-8-sig') as f:
            # CSV 파일을 Dictionary 형태로 읽음
            reader = csv.DictReader(f)
            
            for row in reader:
                try:
                    # 지정된 '헤더 이름'으로 데이터를 가져옴
                    hid_data = row[DATA_COLUMN_NAME].strip()
                except KeyError:
                    print(f"[오류] CSV 파일에 '{DATA_COLUMN_NAME}' 헤더가 없습니다.")
                    print("스크립트 상단의 'DATA_COLUMN_NAME' 변수를 CSV 파일의 실제 헤더 이름과 일치시키세요.")
                    sys.exit(1)
                except TypeError:
                    # 행이 비어있는 경우 등
                    continue

                # 유효한 8바이트(16글자) 데이터인지 확인
                if len(hid_data) != 16:
                    continue

                try:
                    # 16진수 문자열을 잘라서 10진수 정수로 변환
                    modifier = int(hid_data[0:2], 16) # 1번째 바이트 (Shift)
                    keycode = int(hid_data[4:6], 16)  # 3번째 바이트 (Key)
                except ValueError:
                    # 16진수 변환이 불가능한 데이터
                    continue

                # 키가 릴리즈(떼어짐)된 패킷 (keycode가 0)
                # 예: 0000000000000000
                if keycode == 0x00:
                    pressed_keys.clear() # 눌린 키 상태를 초기화
                    continue

                # 키를 계속 누르고 있어서 발생하는 중복 패킷은 무시
                if keycode in pressed_keys:
                    continue
                
                # 새로운 키 입력으로 처리
                pressed_keys.add(keycode)

                if keycode in SCANCODE_MAP:
                    # 쉬프트 키가 눌렸는지 확인 (Left Shift: 0x02, Right Shift: 0x20)
                    is_shift = (modifier == 0x02) or (modifier == 0x20)
                    
                    char_pair = SCANCODE_MAP[keycode]
                    
                    # (오류 수정) char_to_pair -> char_pair
                    char_to_add = char_pair[1] if is_shift else char_pair[0]
                    result_chars.append(char_to_add)

    except FileNotFoundError:
        print(f"[오류] '{CSV_FILE}' 파일을 찾을 수 없습니다.")
        print(r"파일 경로에 백슬래시(\)가 있다면, 경로 앞에 r을 붙여주세요.")
        print(r"예: CSV_FILE = r'C:\Users\user\captures.csv'")
        sys.exit(1)
    except UnicodeDecodeError:
        print(f"[오류] 파일 인코딩 오류. '{CSV_FILE}' 파일을 'utf-8'로 열 수 없습니다.")
        print("스크립트의 'encoding' 값을 'cp949' (한글 윈도우 기본값)로 변경해 보세요.")
        sys.exit(1)
    except Exception as e:
        print(f"예상치 못한 오류 발생: {e}")
        sys.exit(1)

    # --- 결과 출력 ---
    try:
        with open(OUTPUT_FILE, "w", encoding="utf-8") as f:
            f.write("".join(result_chars))
        
        print("--------------------------")
        print(f"✅ 추출 완료!")
        print(f"결과를 '{OUTPUT_FILE}' 파일에 저장했습니다.")
        print("--------------------------")

    except Exception as e:
        print(f"결과 파일 저장 중 오류 발생: {e}")


if __name__ == "__main__":
    main()