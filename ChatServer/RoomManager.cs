using ServerCoreTCP.Utils;
using System;
using System.Collections.Generic;

namespace ChatServer
{
    class RoomManager
    {
        const int FlushTickInterval = 500;

        #region Singleton
        readonly static RoomManager _instance = new RoomManager();
        public static RoomManager Instance => _instance;
        #endregion

        public IReadOnlyDictionary<uint, Room> Rooms => _rooms;
        Dictionary<uint, Room> _rooms = new();
        readonly static object _lock = new object();
        public void FlushRoom()
        {
            foreach (var room in _rooms.Values)
            {
                if (room.IsActivate == true)
                {
                    room.AddJob(() => room.Flush());
                }
            }
            JobTimer.Instance.Push(FlushRoom, FlushTickInterval);
        }

        public bool TryCreateNewRoom(uint roomId, uint userId)
        {
            lock (_lock)
            {
                if (_rooms.ContainsKey(roomId) == false)
                {
                    _rooms.Add(roomId, new Room(roomId, userId));
                    return true;
                }
                else return false;
            }
        }

        public bool TryRemoveRoom(uint roomId)
        {
            lock (_lock)
            {
                return _rooms.Remove(roomId);
            }
        }
    }
}
