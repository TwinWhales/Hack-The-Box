# 분석할 텍스트 데이터
data = "QK[LEFT]_[RIGHT].[LEFT][LEFT][LEFT][LEFT]H[RIGHT]5[LEFT][LEFT]{_[LEFT]I[RIGHT][RIGHT]ck[RIGHT]'[RIGHT][RIGHT]b0[LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT]I[LEFT][LEFT][LEFT][LEFT]T[RIGHT][RIGHT]f[RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT]_[RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT]}[LEFT].[LEFT].[LEFT][LEFT][LEFT][LEFT]3[LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT]u[LEFT][LEFT]t_[RIGHT][RIGHT]a[LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT][LEFT]B[RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT]t[RIGHT]5[LEFT][LEFT][LEFT]I[RIGHT][RIGHT][RIGHT]_[RIGHT][RIGHT][RIGHT][RIGHT][RIGHT]a[LEFT][LEFT][LEFT][LEFT][LEFT][LEFT]a[RIGHT][RIGHT][RIGHT][RIGHT][RIGHT][RIGHT]d[LEFT][LEFT][LEFT][LEFT]y[RIGHT][RIGHT][RIGHT]r"

# 플래그를 조립할 리스트 (텍스트 버퍼)
buffer = []

# 현재 커서(삽입) 위치
cursor = 0

# 데이터 문자열을 읽는 인덱스
i = 0

while i < len(data):
    if data[i:].startswith('[LEFT]'):
        # 커서를 왼쪽으로 한 칸 이동 (0보다 작아질 수 없음)
        cursor = max(0, cursor - 1)
        # 읽는 인덱스를 [LEFT]의 길이(6)만큼 점프
        i += len('[LEFT]')
        
    elif data[i:].startswith('[RIGHT]'):
        # 커서를 오른쪽으로 한 칸 이동
        # (커서가 버퍼의 실제 끝보다 더 오른쪽으로 갈 수도 있음)
        cursor += 1
        # 읽는 인덱스를 [RIGHT]의 길이(7)만큼 점프
        i += len('[RIGHT]')
        
    else:
        # 일반 문자일 경우
        char = data[i]
        
        # 현재 커서 위치에 문자를 '삽입(insert)'
        buffer.insert(cursor, char)
        
        # 문자를 삽입했으므로, 텍스트 편집기처럼 커서도 오른쪽으로 한 칸 이동
        cursor += 1
        # 읽는 인덱스 1 증가
        i += 1

# 조립된 리스트(buffer)를 하나의 문자열로 합쳐서 출력
flag = "".join(buffer)
print(f"flag : {flag}")