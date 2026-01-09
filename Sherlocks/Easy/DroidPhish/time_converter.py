from datetime import datetime

# 1. packages.xml에서 가져온 16진수 타임스탬프 문자열
hex_timestamp_str = "1935f2b00f3"

# 2. 16진수 문자열을 10진수 정수(밀리초)로 변환
timestamp_ms = int(hex_timestamp_str, 16)
print(f"16진수 -> 10진수 (밀리초): {timestamp_ms}")

# 3. 밀리초(ms)를 초(s)로 변환 (1초 = 1000밀리초)
timestamp_s = timestamp_ms / 1000
print(f"밀리초 -> 초: {timestamp_s}")

# 4. 유닉스 타임스탬프(초)를 UTC 기준의 datetime 객체로 변환
utc_datetime = datetime.utcfromtimestamp(timestamp_s)

# 5. 사람이 읽기 쉬운 형태로 출력 (YYYY-MM-DD HH:MM:SS)
formatted_utc_time = utc_datetime.strftime("%Y-%m-%d %H:%M:%S")

print(f"\n변환된 UTC 시간: {formatted_utc_time}")