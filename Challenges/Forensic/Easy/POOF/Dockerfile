# 파이썬 3.6 버전의 공식 이미지를 기반으로 사용
FROM python:3.6-slim

# 필요한 패키지 설치
RUN pip install uncompyle6

# pyinstxtractor 스크립트 다운로드
RUN apt-get update && apt-get install -y git
RUN git clone https://github.com/extremecoders-re/pyinstxtractor.git /pyinstxtractor

# 컨테이너의 작업 디렉토리 설정
WORKDIR /app
