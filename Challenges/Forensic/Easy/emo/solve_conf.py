
def solve_conf():
    # 1. PowerShell에 하드코딩된 바이트 배열들 합치기
    # 코드에 흩어져 있는 $FN5ggmsH += ... 부분들을 순서대로 모았습니다.
    static_bytes = [
        182,187,229,146,231,177,151,149,166, # 첫 번째 선언
        186,141,228,182,177,171,229,236,239,239,239,228,181,182,171,229,234,239,239,228, # 두 번째 추가
        185,179,190,184,229,151,139,157,164,235,177,239,171,183,236,141,128,187,235,134,128,158,177,176,139, # 세 번째 추가
        183,154,173,128,175,151,238,140,183,162,228,170,173,179,229 # 네 번째 추가
    ]

    # 2. 난독화된 URL 리스트 (PS 코드에서 가져옴)
    raw_urls = "http:][(s)]w][(s)]wda-industrial.htb][(s)]wjs][(s)]w][(s)]w@http:][(s)]w][(s)]wdaprofesional.htb][(s)]wdata4][(s)]whWgWjTV][(s)]w@https:][(s)]w][(s)]wdagranitegiare.htb][(s)]wwp-admin][(s)]wtV][(s)]w@http:][(s)]w][(s)]wwww.outspokenvisions.htb][(s)]wwp-includes][(s)]waWoM][(s)]w@http:][(s)]w][(s)]wmobsouk.htb][(s)]wwp-includes][(s)]wUY30R][(s)]w@http:][(s)]w][(s)]wbiglaughs.htb][(s)]wsmallpotatoes][(s)]wY][(s)]w@https:][(s)]w][(s)]wngllogistics.htb][(s)]wadminer][(s)]wW3mkB"
    
    # URL 분리 (Delimiter: @) 및 쓰레기 문자 제거
    # PowerShell의 -split 로직을 흉내 냄
    urls = raw_urls.replace("][(s)]w", "").split("@")

    xor_key = 0xDF # PowerShell: -bxor 0xdf

    print(f"[*] 총 {len(urls)}개의 URL에 대해 복호화 시도...\n")

    for i, url in enumerate(urls):
        if not url: continue
        
        # 전체 바이트 구성: Static Bytes + (URL Chars XOR 0xDF) + 228
        current_bytes = list(static_bytes)
        
        # URL 부분 XOR 연산
        for char in url:
            current_bytes.append(ord(char) ^ xor_key)
        
        # 마지막 바이트 추가
        current_bytes.append(228)

        # 복호화 (XOR 0xDF)
        # PowerShell은 XOR해서 저장하고, 나중에 읽을 때 다시 XOR해서 원문을 볼 것입니다.
        # 여기서는 저장되기 전의 '의미 있는 텍스트'를 보기 위해 바이트 자체를 0xDF로 XOR 해봅니다.
        decoded_chars = []
        for b in current_bytes:
            decoded_chars.append(chr(b ^ xor_key))
        
        result_text = "".join(decoded_chars)
        
        print(f"[{i+1}] URL: {url}")
        print(f"    Result: {result_text}")
        print("-" * 50)

if __name__ == "__main__":
    solve_conf()
