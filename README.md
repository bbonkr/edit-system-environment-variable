Edit System Environment Variable <small>시스템 변수 편집기</small>
===============================================================

## 개요

환경 변수를 추가, 변경, 제거하려면 아래 창을 열어 진행합니다.

변수의 값을 입력하는 텍스트박스가 작아서 입력값을 모두 확인하기 어렵습니다.
(e.g. PATH 변수의 값)

이를 좀 더 편리하게 관리하기 위해 시스템 변수 편집기 응용 프로그램을 작성하였습니다.

## 시스템 요구사항

### 지원 OS
* Windows XP SP3
* Windows Server 2003 SP2
* Windows Vista SP1 or later
* Windows Server 2008 (not supported on Server Core Role)
* Windows 7
* Windows Server 2008 R2 (not supported on Server Core Role)
* Windows Server 2008 R2 SP1
* Windows 8
* Windows Server 2012 (not supported on Server Core Role)
* Windows 8.1
* Windows Server 2012 R2 (not supported on Server Core Role)
* Windows 10

### .Net Framework

* .Net framework 4.0

### 권한

로컬 관리자 권한 Administrators Group

## 설명

![실행화면](./@img/p-002.png)

### 버튼

* New: 새로운 변수를 추가합니다.
* Reload: 환경변수를 다시 읽습니다.
* Delete: 리스트에서 선택된 변수를 제거합니다.
* Save: 리스트에서 선택된 변수의 값을 저장합니다.

### 메뉴

* File > Reload: 환경변수를 다시 읽습니다.
* File > New variable: 새로운 변수를 추가합니다.
* File > Save: 리스트에서 선택된 변수의 값을 저장합니다.
* File > Delete: 리스트에서 선택된 변수를 제거합니다.
* Edit > Show Console Window : 체크가 선택된 상태면 화면 아래쪽에 콘솔창을 보여줍니다.

## 제약사항

* 관리자 권한으로 실행해야 합니다. (기본적으로 관리자 권한으로 실행합니다.)
* 시스템 변수만 관리합니다.
* 사용자 변수는 관리하지 않습니다.
