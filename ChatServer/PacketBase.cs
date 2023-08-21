using Chat;
using System;
using System.Collections.Generic;

namespace ServerCoreTCP.MessageWrapper
{
    public class PacketBase
    {
        public readonly static Dictionary<Type, PacketType> PacketMap = new Dictionary<Type, PacketType>()
        {
            { typeof(ChatBase), PacketType.ChatBase },
            { typeof(SChatText), PacketType.SChatText },
            { typeof(SChatIcon), PacketType.SChatIcon },
            { typeof(CRecvChatText), PacketType.CRecvChatText },
            { typeof(CRecvChatIcon), PacketType.CRecvChatIcon },
            { typeof(SReqRoomList), PacketType.SReqRoomList },
            { typeof(CResRoomList), PacketType.CResRoomList },
            { typeof(RoomInfo), PacketType.RoomInfo },
            { typeof(SReqCreateRoom), PacketType.SReqCreateRoom },
            { typeof(CResCreateRoom), PacketType.CResRoomList },
            { typeof(SLeaveRoom), PacketType.SLeaveRoom },
            { typeof(CLeaveRoom), PacketType.CLeaveRoom },
            { typeof(SRemoveRoom), PacketType.SRemoveRoom },
            { typeof(CRemovedRoom), PacketType.CRemovedRoom },
            { typeof(UserInfo), PacketType.UserInfo },
        };
    }

    public enum PacketType
    {
        INVALID = 0,

        UserInfo = 1,
        RoomInfo = 2,
        ChatBase = 3,

        SReqRoomList = 100,
        SReqCreateRoom = 101,
        SLeaveRoom = 102,
        SRemoveRoom = 103,
        SChatText = 110,
        SChatIcon = 111,

        CResRoomList = 200,
        CResCreateRoom = 201,
        CLeaveRoom = 202,
        CRemovedRoom = 203,
        CRecvChatText = 210,
        CRecvChatIcon = 211,

    }
}