using System;

namespace ChatServer.Chat
{
#if PACKET_TYPE_INT
    public enum PacketType : uint
    {
        P_INVALID = 0,
        P_SSendChatText = 1,
        P_SSendChatIcon = 2,
        P_CSendChat = 3,
        P_CChatText = 4,
        P_CSendIcon = 5,
        P_ChatBase = 6,
        P_ChatText = 7,
        P_ChatIcon = 8,
        P_SCreateRoomReq = 9,
        P_CCreateRoomRes = 10,
        P_SEnterRoomReq = 11,
        P_CEnterRoomRes = 12,
        P_SAllRoomListReq = 13,
        P_SRoomListReq = 14,
        P_CRoomListRes = 15,
        P_SLeaveRoomReq = 16,
        P_CUserLeftRoom = 17,
        P_SLoginReq = 18,
        P_CLoginRes = 19,

    }
#else
    public enum PacketType : ushort
    {
        P_INVALID = 0,
        P_SSendChatText = 1,
        P_SSendChatIcon = 2,
        P_CSendChat = 3,
        P_CChatText = 4,
        P_CSendIcon = 5,
        P_ChatBase = 6,
        P_ChatText = 7,
        P_ChatIcon = 8,
        P_SCreateRoomReq = 9,
        P_CCreateRoomRes = 10,
        P_SEnterRoomReq = 11,
        P_CEnterRoomRes = 12,
        P_SAllRoomListReq = 13,
        P_SRoomListReq = 14,
        P_CRoomListRes = 15,
        P_SLeaveRoomReq = 16,
        P_CUserLeftRoom = 17,
        P_SLoginReq = 18,
        P_CLoginRes = 19,

    }
#endif
}
