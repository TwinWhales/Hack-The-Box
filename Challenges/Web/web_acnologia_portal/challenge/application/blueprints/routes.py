import json
from application.database import User, Firmware, Report, db, migrate_db
from application.util import is_admin, extract_firmware
from flask import Blueprint, jsonify, redirect, render_template, request
from flask_login import current_user, login_required, login_user, logout_user
from application.bot import visit_report

web = Blueprint('web', __name__) 
api = Blueprint('api', __name__)
# __name__ : 현재 실행중인 파이썬 모듈의 이름을 나타내는 변수로 직접 실행하면 __main__, import하면 파일명이 됨
# 이를 통해 Flask가 Blueprint의 위치를 올바르게 인식할 수 있음

def response(message):
    return jsonify({'message': message}) # jsonify() : 응답을 json 형태로 변환하는 함수

@web.route('/', methods=['GET'])
def login():
    return render_template('login.html') # render_template() : flask에서 html 파일을 랜더링하는 함수

@api.route('/login', methods=['POST'])
def user_login(): 
    if not request.is_json: # json 파라미터여야됨
        return response('Missing required parameters!'), 401

    data = request.get_json() # json 형식으로 username, password 파라미터를 리퀘스트로 입력받음
    username = data.get('username', '')
    password = data.get('password', '')

    if not username or not password:
        return response('Missing required parameters!'), 401

    user = User.query.filter_by(username=username).first() # Flask-SQLAlchemy 문법
    # User.query : User라는 테이블에서 조회 수행
    # filtered_by(username=username) : username 검색
    # .first() : 일치하는 첫번째 값만 가져옴, 없으면 none 리턴
    if not user or not user.password == password:
        return response('Invalid username or password!'), 403

    login_user(user) # login 상태를 유지하는 함수
    return response('User authenticated successfully!')

@web.route('/register', methods=['GET'])
def register():
    return render_template('register.html')

@api.route('/register', methods=['POST'])
def user_registration():
    if not request.is_json: # 마찬가지로 json 형식으로 입력받음
        return response('Missing required parameters!'), 401

    data = request.get_json()
    username = data.get('username', '')
    password = data.get('password', '')

    if not username or not password:
        return response('Missing required parameters!'), 401

    user = User.query.filter_by(username=username).first()

    if user:
        return response('User already exists!'), 401

    new_user = User(username=username, password=password)
    db.session.add(new_user)
    db.session.commit()

    return response('User registered successfully!')

@web.route('/dashboard')
@login_required # flask에서 제공하는 데코레이터로, 로그인 상태일때만 접근가능하다.
def dashboard():
    return render_template('dashboard.html')

@api.route('/firmware/list', methods=['GET'])
@login_required 
def firmware_list():
    firmware_list = Firmware.query.all()
    return jsonify([row.to_dict() for row in firmware_list])

@api.route('/firmware/report', methods=['POST'])
@login_required
def report_issue():
    if not request.is_json:
        return response('Missing required parameters!'), 401

    data = request.get_json()
    module_id = data.get('module_id', '') # 취약점1 : 입력값 검증없음
    issue = data.get('issue', '') # 취약점2 : 입력값 검증없음

    if not module_id or not issue:
        return response('Missing required parameters!'), 401

    new_report = Report(module_id=module_id, issue=issue, reported_by=current_user.username)
    db.session.add(new_report) # session.add() : SQLAlchemy에서 새로운 데이터를 데이터베이스에 추가 (대기)
    db.session.commit() # session.comit() : 데이터베이스 변경내용 반영

    visit_report()
    migrate_db()

    return response('Issue reported successfully!')

@api.route('/firmware/upload', methods=['POST'])
@login_required
@is_admin
def firmware_update():
    if 'file' not in request.files:
        return response('Missing required parameters!'), 401

    extraction = extract_firmware(request.files['file'])
    if extraction:
        return response('Firmware update initialized successfully.')

    return response('Something went wrong, please try again!'), 403

@web.route('/review', methods=['GET'])
@login_required
@is_admin
def review_report():
    Reports = Report.query.all()
    return render_template('review.html', reports=Reports)

@web.route('/logout')
@login_required
def logout():
    logout_user()
    return redirect('/')
