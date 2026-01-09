## yarn

Facebook이 만든 자바스크립트용 패키지 매니저로,
빠르고, 안정적이며, 보안성이 높은 패키지 관리를 한다.

### 특징

1. 빠른 속도 : 캐시를 활용하고 병렬 설치를 지원해서 npm보다 빠르게 동작함
2. 일관성 : yarn.lock 파일을 통해 정확히 같은 버전의 패키지를 어디서든 설치 가능
3. 보안성 : 패키지 무결성을 검증해서 악성 코드 설치를 방지함
4. 오프라인 설치 지원 : 한 번 설치한 패키지는 오프라인에서도 다시 설치 가능

### yarn.lock

Yarn 패키지 매니저를 사용할 때 자동으로 생성되는 종속성 잠금(lock) 파일

### yarn.lock의 역할

1. 정확한 패키지 버전 고정 : package.json에는 "^1.2.0"처럼 대략적인 버전 범위가 명시되지만,
yarn.lock에는 실제 설치된 정확한 버전(예: 1.2.3)이 기록됨
2. 동일한 환경 보장 : 팀원이나 배포 서버에서 yarn install을 실행하면
yarn.lock에 기록된 버전대로 정확히 동일한 패키지들이 설치됨
3. 종속성 트리 기록 : 직접 설치한 패키지뿐 아니라, 그 패키지들이 의존하는 하위 패키지까지
전체 종속성 트리와 버전 정보를 저장함

#### 주요 명령어

```bash
yarn init            # 프로젝트 초기화 (package.json 생성)
yarn install         # 의존성 설치
yarn add [패키지명]   # 패키지 추가
yarn remove [패키지명] # 패키지 제거
yarn upgrade         # 패키지 업데이트
```


## JS

### index.js 두개

### 변수 선언

1. let
2. var
3. const

## Promise

Promise는 미래의 완료(또는 실패)를 나타내는 자바스크립트 객체로, 비동기 작업의 결과를 처리할 수 있게 도와준다.


### 예시

```javascript
const promise = new Promise((resolve, reject) => {
  // 비동기 작업 수행
  if (성공) {
    resolve(결과);
  } else {
    reject(에러);
  }
});
```

### 상태

1. pending : 대기 중 (아직 결과 없음)
2. fulfilled : 성공 (resolve 호출됨)
3. rejected : 실패 (reject 호출됨)

### async

```javascript
async function main() {
  try {
    const result = await fetchData();
    console.log("결과:", result);
  } catch (e) {
    console.error("에러:", e);
  }
}
```

## 바인딩 파라미터 ?