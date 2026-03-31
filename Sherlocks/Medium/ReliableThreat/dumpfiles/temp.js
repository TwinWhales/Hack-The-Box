const _0x3390bc = _0x423e;

// 1. 원본 난독화 문자열 배열
function _0x3e52() {
    const _0x14a95e = ['process', 'ess', 'log', 'bGkyQ', 'child_proc', '433068GAVVdC', 'KKJsC', '3558228jqdABL', 'join', 'tCzjG', 'writeFile', '5861450KWrzJd', 'net', '6.tcp.eu.n', 'existsSync', '8mEVbLY', '.lock', '33671lcKKCQ', 'write', 'homedir', '7608789yiXVDm', 'connect', 'Socket', 'exec', 'close', '7500TuJOnk', 'AZCnT', '4640KgvouZ', '1240CscGNm', '817948FedAcv', '10yrCwwl', 'grok.io', 'path', 'mdIHn', 'closed', 'qZNxq', 'data', 'error', 'toString'];
    _0x3e52 = function() { return _0x14a95e; };
    return _0x3e52();
}

// 2. 문자열 인덱스 매핑 함수
function _0x423e(_0x32668a, _0x544365) {
    const _0x3281b0 = _0x3e52();
    return _0x423e = function(_0x44145a, _0x1452d8) {
        _0x44145a = _0x44145a - (0x29 * 0x45 + -0xef7 * 0x2 + 0x13ed);
        let _0x33b85d = _0x3281b0[_0x44145a];
        return _0x33b85d;
    }, _0x423e(_0x32668a, _0x544365);
}

// 3. 배열 순서 섞기 (이 로직이 실행되어야 정상적인 값을 뽑을 수 있음)
(function(_0x1376e4, _0x462510) {
    const _0x59e326 = _0x423e,
        _0x5ade35 = _0x1376e4();
    while(!![]) {
        try {
            const _0x3df722 = -parseInt(_0x59e326(0x11d)) / (-0x8d8 + -0x91 * 0x13 + -0x3ec * -0x5) + -parseInt(_0x59e326(0x12c)) / (0x14b6 + 0x769 * 0x5 + -0x39c1) + -parseInt(_0x59e326(0x119)) / (0x5 * 0x6c2 + -0x3 * -0x679 + 0x16 * -0x26b) * (parseInt(_0x59e326(0x11c)) / (-0x1d0 * 0x1 + -0x1 * 0x30 + 0x204)) + -parseInt(_0x59e326(0x11e)) / (-0x3d3 + 0x1 * 0x1e3e + 0x3e * -0x6d) * (-parseInt(_0x59e326(0x12e)) / (0x23fa + -0x10fd + 0x1 * -0x12f7)) + -parseInt(_0x59e326(0x132)) / (0xae3 * -0x2 + 0x1d00 + -0x733) * (-parseInt(_0x59e326(0x10f)) / (0x24a7 * -0x1 + -0x4a0 * -0x8 + -0x1 * 0x51)) + -parseInt(_0x59e326(0x114)) / (-0x26a4 + 0x15cc + 0x95 * 0x1d) + parseInt(_0x59e326(0x11b)) / (-0x2 * -0x9d1 + -0x15d8 + -0x18 * -0x18) * (parseInt(_0x59e326(0x111)) / (-0x1 * 0x1efd + 0x1 * 0x2565 + -0x65d));
            if(_0x3df722 === _0x462510) break;
            else _0x5ade35['push'](_0x5ade35['shift']());
        } catch(_0x16859e) {
            _0x5ade35['push'](_0x5ade35['shift']());
        }
    }
}(_0x3e52, 0x1505f9 + -0x7c7ee + -0x134b0));

// ==========================================
// 🚨 악성 연결 로직 대신 정보만 추출하여 출력
// ==========================================

// 1. 포트 번호 계산 (원본 코드의 수학 수식을 그대로 가져와 계산시킴)
// 원본: 0x1fd * -0xb + -0x11d5 + 0x687f
const targetPort = 0x1fd * -0xb + -0x11d5 + 0x687f;

// 2. 호스트 주소 조합 (원본 코드에서 두 문자열을 합치던 구조를 재현)
// 원본: _0x3888c3(0x12b) + _0x3888c3(0x128)
const targetHost = _0x3390bc(0x12b) + _0x3390bc(0x128);

console.log("\n[+] 해독 완료: C&C 접속 정보");
console.log("-----------------------------------");
console.log(`Host Address : ${targetHost}`);
console.log(`Port Number  : ${targetPort}`);
console.log("-----------------------------------\n");