using Google.Protobuf;
using ServerCoreTCP.Utils;
using ServerCoreTCP.MessageWrapper;
using System;
using System.Collections.Generic;

using Chat;
using Google.Protobuf.WellKnownTypes;

namespace ChatServer
{
    public class Room
    {
        public uint OwnUser => _ownUser;
        uint _ownUser = 0;
        public bool IsActivate => _pendingMessages.Count != 0;
        public uint RoomNo => _roomNo;
        readonly uint _roomNo;

        readonly Dictionary<uint, ClientSession> _users = new();
        readonly JobQueue _jobs = new JobQueue();
        readonly List<ArraySegment<byte>> _pendingMessages = new List<ArraySegment<byte>>();

        readonly object _roomLock = new();
        readonly object _sessionLock = new();
        readonly object _pendingQueueLock = new();

        public Room(uint roomId, uint ownerId)
        {
            _roomNo = roomId;
            _ownUser = ownerId;
        }

        ~Room()
        {
            Program.ConsoleLogger.Information("[Destructor]The room [id={_roomNo}] is removed.", _roomNo);
            Program.Logger.Information("[Destructor]The room [id={_roomNo}] is removed.", _roomNo);
        }

        public IReadOnlyList<uint> GetUsers()
        {
            lock(_roomLock)
            {
                return new List<uint>(_users.Keys);
            }
        }

        public void AddJob(Action job)
        {
            _jobs.Add(job);
        }

        public void Flush()
        {
            foreach (ClientSession session in _users.Values)
            {
                session.SendRaw(_pendingMessages);
            }

            _pendingMessages.Clear();
        }

        public bool Enter(ClientSession session)
        {
            if (_users.ContainsKey(session.User.Id)) return false;

            lock (_sessionLock)
            {
                _users.Add(session.User.Id, session);
            }

            session.EnterRoom(this);

            // broadcast
            CNewUserEnterRoom msg = new()
            {
                NewUser = session.User.UserInfo,
                RoomId = RoomNo
            };
            BroadCast(msg);
            return true;
        }

        public void Leave(ClientSession session)
        {
            lock (_sessionLock)
            {
                _users.Remove(session.User.Id);

                session.LeaveRoom(this);

                if (_users.Count == 0)
                {
                    if (RoomManager.Instance.TryRemoveRoom(RoomNo))
                    {
                        Program.ConsoleLogger.Information("The room [id={RoomNo}] is removed", RoomNo);
                    }
                    else
                    {
                        Program.ConsoleLogger.Error("Trying to remove room; Can not find room [id={RoomNo}]", RoomNo);
                    }
                }
            }

            CLeaveRoom msg = new()
            {
                RoomId = RoomNo,
                UserInfo = session.User.UserInfo
            };
            BroadCast(msg);
        }

        public void SendChatText(ClientSession session, string msg)
        {
            ChatBase chat = new();
            chat.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);
            chat.ChatType = ChatType.Text;

            CRecvChatText res = new()
            {
                RoomId = RoomNo,
                SenderInfo = session.User.UserInfo,
                ChatBase = chat,
                Msg = msg,
            };
            BroadCast(res);
        }

        public void SendChatIcon(ClientSession session, uint iconId)
        {
            ChatBase chat = new();
            chat.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);
            chat.ChatType = ChatType.Icon;

            CRecvChatIcon res = new()
            {
                RoomId = RoomNo,
                SenderInfo = session.User.UserInfo,
                ChatBase = chat,
                IconId = iconId
            };
            BroadCast(res);
        }

        public void BroadCast(ArraySegment<byte> buffer)
        {
            lock (_pendingQueueLock)
            {
                _pendingMessages.Add(buffer);
            }
        }

        public void BroadCast<T>(T message) where T : IMessage
        {
            var type = typeof(T);

            Program.ConsoleLogger.Information("[BroadCast] {type} {message}", type, message);
            lock (_pendingQueueLock)
            {
                _pendingMessages.Add(message.SerializeWrapper());
            }
        }
    }
}
