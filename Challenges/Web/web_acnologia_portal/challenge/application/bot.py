from selenium import webdriver
from flask import current_app
import time

# XSS 를 위한 관리자 로그인

def visit_report():
    chrome_options = webdriver.ChromeOptions()

    chrome_options.add_argument('--headless') # --headless : 백그라운드 동작
    chrome_options.add_argument("--incognito") # --incognito : 시크릿모드(쿠키 X)
    chrome_options.add_argument('--no-sandbox') # 보안기능 비활성화 1
    chrome_options.add_argument('--disable-setuid-sandbox') # 보안기능 비활성화 2
    chrome_options.add_argument('--disable-gpu') # GPU 가속 비활성화
    chrome_options.add_argument('--disable-dev-shm-usage')
    chrome_options.add_argument('--disable-background-networking') # 백그라운드 네트워킹 기능 비활성화
    chrome_options.add_argument('--disable-extensions')
    chrome_options.add_argument('--disable-sync') # 브라우저 기능 최소화 (1)
    chrome_options.add_argument('--disable-translate') # 브라우저 기능 최소화 (2)
    chrome_options.add_argument('--metrics-recording-only')
    chrome_options.add_argument('--mute-audio') # 오디오기능 비활성화
    chrome_options.add_argument('--no-first-run')
    chrome_options.add_argument('--safebrowsing-disable-auto-update')
    chrome_options.add_argument('--js-flags=--noexpose_wasm,--jitless') # WebAssembly 및 JIT 컴파일러 비활성화

    client = webdriver.Chrome(chrome_options=chrome_options)
    client.set_page_load_timeout(5) # 페이지 로드 최대 5초 대기 
    client.set_script_timeout(5) # 자바 스크립트 실행 최대 5초 대기

    client.get('http://localhost:1337/')

    username = client.find_element_by_id('username')
    password = client.find_element_by_id('password')
    login = client.find_element_by_id('login-btn')

    username.send_keys(current_app.config['ADMIN_USERNAME'])
    password.send_keys(current_app.config['ADMIN_PASSWORD'])
    login.click()
    time.sleep(3)

    client.get('http://localhost:1337/review')

    time.sleep(3)
    client.quit()