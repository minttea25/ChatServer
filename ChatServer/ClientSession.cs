using Chat;
using ChatServer.Data;
using ServerCoreTCP.MessageWrapper;
using System;
using System.Collections.Generic;
using System.Net;

namespace ChatServer
{
    public class ClientSession : PacketSession
    {
        public readonly uint SessionId;
        public User User => _user;
        public IReadOnlyDictionary<uint, Room> Rooms => _rooms;

        User _user;
        readonly Dictionary<uint, Room> _rooms = new();

        public ClientSession(uint sessionId)
        {
            SessionId = sessionId;
        }

        public void SetUser(User user)
        {
            _user = user;
        }

        public void EnterRoom(Room room)
        {
            _rooms.Add(room.RoomNo, room);
        }

        public void LeaveRoom(Room room)
        {
            _ = _rooms.Remove(room.RoomNo);
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Program.Logger.Information("[sid={SessionId}] OnConnected: {endPoint}", SessionId, endPoint);
            Program.ConsoleLogger.Information("[sid={SessionId}] OnConnected: {endPoint}", SessionId, endPoint);
        }

        public override void OnDisconnected(EndPoint endPoint, object error = null)
        {
            SessionManager.Instance.Remove(this);

            Program.Logger.Information("[sid={SessionId}] OnDisconnected: {endpoint}", SessionId, endPoint);
            Program.ConsoleLogger.Information("[sid={SessionId}] OnDisconnected: {endpoint}", SessionId, endPoint);
        }

        public override void OnRecv(ReadOnlySpan<byte> buffer)
        {
            MessageManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
        }
    }
}
