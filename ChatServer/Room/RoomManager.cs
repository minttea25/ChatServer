﻿using ChatServer.Chat;
using ServerCoreTCP.Job;
using ServerCoreTCP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public partial class RoomManager : JobSerializerWithTimer, IUpdate
    {
        #region Singleton
        static RoomManager _instance = new RoomManager();
        public static RoomManager Instance { get { return _instance; } }
        #endregion

        const int CheckEmptyRoomIntervalMilliseconds = 1000;

        public Dictionary<ulong, Room> Rooms = new Dictionary<ulong, Room>();

        public void Update()
        {
            Flush();

            foreach (Room room in Rooms.Values)
            {
                room.Update();
            }
        }

        public RoomManager()
        {
            AddAfter(CheckAndRemoveEmptyRoomsTimer, CheckEmptyRoomIntervalMilliseconds);
        }

        public List<RoomInfo> GetRooms()
        {
            return Rooms.Values.Select(r => r.RoomInfo).ToList();
        }

        Room CreateRoom(ulong roomId, string name)
        {
            if (Rooms.ContainsKey(roomId) == true) return null;

            Room room = new Room(roomId, name);
            Rooms.Add(roomId, room);
            return room;
        }



        void CheckAndRemoveEmptyRoomsTimer()
        {
            CheckAndRemoveEmptyRooms();
            AddAfter(CheckAndRemoveEmptyRoomsTimer, CheckEmptyRoomIntervalMilliseconds);
        }

        /// <summary>
        /// 비어있는 방 확인 후 삭제
        /// </summary>
        void CheckAndRemoveEmptyRooms()
        {
            //Console.WriteLine("CheckAndRemoveEmptyRooms");
            foreach (var roomId in Rooms.Keys.ToList())
            {
                if (Rooms[roomId].UserCount == 0) Rooms.Remove(roomId);
            }
        }
    }
}
