# 취약점 분석

## hackattemp.html

```html
    <div class="container">
        <h1>Hacking Attempt Detected</h1>
        <p>Your IP address has been flagged, a report with your browser information has been sent to the administrators for investigation.</p>
        <p><strong>Client Request Information:</strong></p>
        <pre>{{ request_info | safe}}</pre>
    </div>
```

{request_info | safe} 부분이 바로 출력되게 되어있음

## app.py

```python
    @app.route('/dashboard', methods=['GET', 'POST'])
    def admin():
        if serializer.loads(request.cookies.get('is_admin')) == "user":
            return abort(401)

        script_output = ""

        if request.method == 'POST':
            date = request.form.get('date')
            if date:
                script_output = os.popen(f'bash report.sh {date}').read()

        return render_template('dashboard.html', script_output=script_output)
```

script_output = os.popen(f'bash report.sh {date}').read()에서 date를 아무런 필터없이 입력받고있음

## shellcheck

```sh
    if ! /usr/bin/pgrep -x "initdb.sh" &>/dev/null; then
    /usr/bin/echo "Database service is not running. Starting it..."
    ./initdb.sh 2>/dev/null
```

initdb.sh의 경로가 명확하지않음