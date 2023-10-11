##### Some AI translations are included.

# ChatServer
This is the server program associated with the [ChatClient](https://github.com/minttea25/ChatClient) project designed to test [ServerCoreTCP](https://github.com/minttea25/TCPServerCore).

# Built Framework Version
`.NET 5.0`

# Features
## Classes
- `Program (including Main function)`: You can enter commands related to server execution here.
- `NetworkManager`, `Server`: Central managers of the server program, including start, stop, and session creation.
- `SessionManager`: Manages sessions of currently connected clients.
- `DataManager`: Manages IO operations on the data recorded on the server and issues IDs. It holds runtime user information.
- `RoomManager`: Manages rooms currently created on the server. Broadcasts user messages received in each room. Deletes rooms with no users left.
- `MessageHandler`: Processes deserialized packet data from the MessageManager.
- `MessageManager`: Deserializes packets received from the RecvBuffer, classifies packets, and calls functions of the MessageHandler based on their types.
## Logging
- Records logs to both the console and text files using Serilog.

# Used External Libraries
- [ServerCoreTCP](https://github.com/minttea25/TCPServerCore)
- [Google.Protobuf](https://protobuf.dev/)
- [Serilog](https://serilog.net/) (File, Console)

---

# ChatServer
ServerCoreTCP를 테스트하기 위한 ChatClient 프로젝트와 관련된 서버 프로그램입니다.

# Built Framework Version
`.NET 5.0`

# Features
## Classes
- `Program (Main 함수 포함)`: 서버 실행에 관련한 명령어를 입력할 수 있습니다.
- `NetworkManager`, `Server`: 서버 프로그램의 중심이 되는 매니저로, 시작, 종료, 세션 생성을 포함합니다.
- `SessionManager`: 현재 연결된 클라이언트의 세션들을 관리합니다.
- `DataManager`: 서버에 기록되어 있는 데이터에 대한 IO 작업 및 id 발급을 해주는 매니저입니다. 현재 런타임 중에만 유지되는 유저 정보를 가지고 있습니다.
- `RoomManager`: 현재 서버에 생성된 방을 관리하는 매니저입니다. 여기서 각 방(room)에 수신된 유저의 메시지를 broadcast 합니다. 방에 남아있는 유저가 한 명도 없을 경우 방을 삭제합니다.
- `MessageHandler`: MessageManager에서 역직렬화된 패킷 데이터를 처리합니다.
- `MessageManager`: RecvBuffer로부터 전달받은 패킷을 역직렬화하여 패킷을 분류하고 타입에 맞게 MessageHandler의 함수를 호출합니다.
## Logging
- Serilog를 이용해 Console과 텍스트 파일에 모두 기록합니다.

# 사용 외부 라이브러리
- [ServerCoreTCP](https://github.com/minttea25/TCPServerCore)
- [Google.Protobuf](https://protobuf.dev/)
- [Serilog](https://serilog.net/) (File, Console)
