using System;

using ServerCoreTCP;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace ChatServer.Chat
{
    public class MessageHandler
    {
        public static void SSendChatTextMessageHandler(IMessage message, Session session)
        {
            SSendChatText msg = message as SSendChatText;

            // TODO
        }

        public static void SSendChatIconMessageHandler(IMessage message, Session session)
        {
            SSendChatIcon msg = message as SSendChatIcon;

            // TODO
        }

        public static void SCreateRoomReqMessageHandler(IMessage message, Session session)
        {
            SCreateRoomReq msg = message as SCreateRoomReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            RoomManager.Instance.HandleCreateRoom(cs, msg);
        }

        public static void SEnterRoomReqMessageHandler(IMessage message, Session session)
        {
            SEnterRoomReq msg = message as SEnterRoomReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            RoomManager.Instance.HandleEnterRoom(cs, msg.RoomId);
        }

        public static void SAllRoomListReqMessageHandler(IMessage message, Session session)
        {
            SAllRoomListReq msg = message as SAllRoomListReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            CRoomListRes res = new();
            foreach(var roomInfo in RoomManager.Instance.GetRooms())
            {
                res.Rooms.Add(roomInfo);
            }
            res.LoadTime = Timestamp.FromDateTime(DateTime.UtcNow);
            // �ٷ� ����
            cs.Send(res);
        }

        public static void SRoomListReqMessageHandler(IMessage message, Session session)
        {
            SRoomListReq msg = message as SRoomListReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            cs.HandleRoomListReq();
        }

        public static void SLeaveRoomReqMessageHandler(IMessage message, Session session)
        {
            // TODO: SLeaveRoom�� �ܼ��� '�� �̹� �����ſ���~' �ϰ� ������ ��Ŷ����
            // res ��Ŷ ���� ����
            // req ���̻� ����
            SLeaveRoomReq msg = message as SLeaveRoomReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            RoomManager.Instance.HandleLeaveRoom(cs, msg.RoomId);
            
        }

        public static void SLoginReqMessageHandler(IMessage message, Session session)
        {
            SLoginReq msg = message as SLoginReq;
            ClientSession cs = session as ClientSession;

            // TEMP
            Console.WriteLine(msg);
            cs.UserInfo = msg.UserInfo;
            CLoginRes res = new()
            {
                LoginRes = LoginRes.LoginSuccess,
            };
            cs.Send(res);
        }


    }
}
