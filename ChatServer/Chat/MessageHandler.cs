using System;

using ServerCoreTCP;
using Google.Protobuf;
using ChatServer;
using ChatServer.Data;
using Google.Protobuf.WellKnownTypes;

namespace Chat
{
    public class MessageHandler
    {
        public static void Log<T>(T message) where T : IMessage
        {
            var t = typeof(T);
            Program.ConsoleLogger.Information("[Recv] {t} {message}", t, message);
        }

        public static void SEnterRoomMessageHandler(IMessage message, Session session)
        {
            SEnterRoom msg = message as SEnterRoom;
            ClientSession s = session as ClientSession;

            Log(msg);

            if (RoomManager.Instance.Rooms.TryGetValue(msg.RoomId, out var room))
            {
                if (room.Enter(s) == true)
                {
                    CEnterRoom res = new()
                    {
                        UserInfo = s.User.UserInfo,
                        RoomId = msg.RoomId,
                        Res = EnterRoomRes.Ok
                    };
                    ServerLogger.Send(s, res);
                }
                else
                {
                    CEnterRoom res = new()
                    {
                        UserInfo = s.User.UserInfo,
                        RoomId = msg.RoomId,
                        Res = EnterRoomRes.AlreadyIn
                    };
                    ServerLogger.Send(s, res);
                }
            }
            else
            {
                CEnterRoom res = new()
                {
                    UserInfo = s.User.UserInfo,
                    RoomId = msg.RoomId,
                    Res = EnterRoomRes.Error
                };
                ServerLogger.Send(s, res);
            }
        }

        public static void ChatBaseMessageHandler(IMessage message, Session session)
        {
            ChatBase msg = message as ChatBase;

            // TODO
        }

        public static void SChatTextMessageHandler(IMessage message, Session session)
        {
            SChatText msg = message as SChatText;
            ClientSession s = session as ClientSession;

            Log(msg);

            var roomId = msg.RoomId;
            if (s.Rooms.ContainsKey(roomId) == false)
            {
                Program.ConsoleLogger.Error("Can not find key={roomId} in ClientSession.Rooms", roomId);
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.NotMatchSession,
                };
                //s.Send(res);
                ServerLogger.Send(s, res);
                return;
            }

            if (RoomManager.Instance.Rooms.TryGetValue(roomId, out var room))
            {
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.Success,
                };
                //s.Send(res);
                ServerLogger.Send(s, res);
                room.SendChatText(s, msg.Msg);
            }
            else
            {
                Program.ConsoleLogger.Error("Can not find key={roomId} in RoomManager.Rooms", roomId);
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.NoSuchRoom,
                };
                //s.Send(res);
                ServerLogger.Send(s, res);
                return;
            }
        }

        public static void SChatIconMessageHandler(IMessage message, Session session)
        {
            SChatIcon msg = message as SChatIcon;
            ClientSession s = session as ClientSession;

            Log(msg);

            var roomId = msg.RoomId;
            if (s.Rooms.ContainsKey(roomId) == false)
            {
                Program.ConsoleLogger.Error("Can not find key={roomId} in ClientSession.Rooms", roomId);
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.NotMatchSession,
                };
                ServerLogger.Send(s, res);
                return;
            }

            if (RoomManager.Instance.Rooms.TryGetValue(roomId, out var room))
            {
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.Success,
                };
                ServerLogger.Send(s, res);
                room.SendChatIcon(s, msg.IconId);
            }
            else
            {
                Program.ConsoleLogger.Error("Can not find key={roomId} in RoomManager.Rooms", roomId);
                CResSendChat res = new()
                {
                    SenderId = s.User.Id,
                    Error = SendChatError.NoSuchRoom,
                };
                ServerLogger.Send(s, res);
                return;
            }
        }

        public static void SReqRoomListMessageHandler(IMessage message, Session session)
        {
            SReqRoomList msg = message as SReqRoomList;
            ClientSession s = session as ClientSession;

            Log(msg);


            CResRoomList res = new();
            res.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(9)); // Use UtcNow (not Now) => ArgumentException
            var list = s.Rooms;
            foreach (var r in list.Keys)
            {
                if (RoomManager.Instance.Rooms.TryGetValue(r, out var room))
                {
                    RoomInfo info = new()
                    {
                        RoomId = r,
                        RoomType = RoomType.Normal,
                        OwnUser = room.OwnUser,
                    };
                    foreach (uint user in room.GetUsers())
                    {
                        info.Users.Add(user);
                    }
                    res.RoomInfos.Add(info);
                }
                else
                {
                    Program.Logger.Error("[MessageHandler] SReqRoomListMessageHandler: There is no key={r} in the RoomManager", r);
                }
            }
            ServerLogger.Send(s, res);
        }

        public static void RoomInfoMessageHandler(IMessage message, Session session)
        {
            RoomInfo msg = message as RoomInfo;

            // TODO
        }

        public static void SReqCreateRoomMessageHandler(IMessage message, Session session)
        {
            SReqCreateRoom msg = message as SReqCreateRoom;
            ClientSession s = session as ClientSession;

            Log(msg);

            if (msg.RoomInfo.RoomId <= 0)
            {
                CResCreateRoom res = new()
                {
                    ReqRoomInfo = msg.RoomInfo,
                    Res = CreateRoomRes.InvalidRoomId
                };
                ServerLogger.Send(s, res);
            }
            else if (RoomManager.Instance.TryCreateNewRoom(msg.RoomInfo.RoomId, msg.UserInfo.UserId))
            {
                CResCreateRoom res = new()
                {
                    ReqRoomInfo = msg.RoomInfo,
                    Res = CreateRoomRes.Ok
                };
                ServerLogger.Send(s, res);
            }
            else
            {
                CResCreateRoom res = new()
                {
                    ReqRoomInfo = msg.RoomInfo,
                    Res = CreateRoomRes.DuplicatedRoomId
                };
                ServerLogger.Send(s, res);
            }

            // TODO
        }

        public static void SLeaveRoomMessageHandler(IMessage message, Session session)
        {
            SLeaveRoom msg = message as SLeaveRoom;
            ClientSession s = session as ClientSession;

            Log(msg);

            var rooms = s.Rooms;
            foreach (uint id in rooms.Keys)
            {
                if (id == msg.RoomId)
                {
                    if (RoomManager.Instance.Rooms.TryGetValue(id, out var room))
                    {
                        room.Leave(s);
                    }
                    else
                    {
                        Program.ConsoleLogger.Error("Can not find key={id} in the RoomManager.Rooms", id);
                    }
                    return;
                }
            }
            Program.ConsoleLogger.Error("Can not find key={RoomId} in the ClientSession.Rooms", msg.RoomId);

        }

        public static void SRemoveRoomMessageHandler(IMessage message, Session session)
        {
            SRemoveRoom msg = message as SRemoveRoom;
            ClientSession s = session as ClientSession;

            Log(msg);

            if (msg.RoomId == 0)
            {
                CRemovedRoom res = new()
                {
                    RoomId = 0,
                };
                ServerLogger.Send(s, res);
            }
            else if (RoomManager.Instance.TryRemoveRoom(msg.RoomId))
            {
                CRemovedRoom res = new()
                {
                    RoomId = msg.RoomId
                };
                ServerLogger.Send(s, res);
            }
            else
            {
                CRemovedRoom res = new()
                {
                    RoomId = 0,
                };
                ServerLogger.Send(s, res);
            }
        }

        public static void UserInfoMessageHandler(IMessage message, Session session)
        {
            UserInfo msg = message as UserInfo;



        }

        public static void SUserAuthReqMessageHandler(IMessage message, Session session)
        {
            SUserAuthReq msg = message as SUserAuthReq;
            ClientSession s = session as ClientSession;

            Log(msg);

            User user = DataManager.Instance.AddNewUser(msg.UserName);
            if (user != null)
            {
                s.SetUser(user);
                CUserAuthRes res = new CUserAuthRes()
                {
                    UserInfo = new() { UserId = user.Id, UserName = user.UserName },
                    AuthRes = UserAuthRes.UserAuthOk,
                };
                ServerLogger.Send(s, res);
            }
            else
            {
                CUserAuthRes res = new CUserAuthRes()
                {
                    UserInfo = new() { UserId = 0, UserName = "" },
                    AuthRes = UserAuthRes.UserAuthDuplicatedName,
                };
                ServerLogger.Send(s, res);
            }
        }
    }
}
