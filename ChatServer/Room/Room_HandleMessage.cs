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
    public partial class Room : JobSerializer, IUpdate
    {
        public void HandleEnterRoom(ClientSession session)
        {
            Add(() =>
            {
                if (_users.ContainsKey(session.UserInfo.UserId) == true)
                {
                    CEnterRoomRes res = new()
                    {
                        Res = EnterRoomRes.EnterRoomAlreadyIn,
                        RoomInfo = RoomInfo
                    };
                    session.Send(res);
                }
                else
                {
                    // enter room
                    UserEnterRoom(session);
                    CEnterRoomRes res = new()
                    {
                        Res = EnterRoomRes.EnterRoomOk,
                        RoomInfo = RoomInfo
                    };
                    session.Send(res);
                }
            });
        }

        public void HandleLeaveRoom(UserInfo leftUser)
        {
            // TODO : 굳이 한번어 add하여 job을 추가할 필요가 있을까? (어차피 하나의 스레드에서 실행됨)
            // Room에서 실행하도록 일을 위임한다는 뜻이면 괜찮을거 같기도 한데
            // Add 자체에 lock을 쓰기 때문에 좀 더 생각해봐야 할 수도...
            // 만약 Add를 안하고 곧바로 실행한다면, 
            // 모든 Room에 관련한 메서드들을 RoomManager에서 처리하고 Room은 Add 없이 가는걸로?
            // 일단 Room도 JobSerializer이고 Update 대상에 포함이 되어있긴 함
            // RoomManager를 거치지 않고 바로 Room에서 뭘 시킬 생각이면
            // Add를 이용하는 함수와 곧바로 실행시키는 함수를 다 포함해야됨

            // => 외부에서 호출할때 직접 Add를 호출하는 방법도 있긴 함
            // 그러면 일관되게 Room이나 RoomManager에서 Add를 호출하는 일은 없는게 좋을듯?
            // 근데 이방법은 실수로 Add 하지 않고 곧바로 호출하게 되면 로직 관련 메서드들이
            // 여러 스레드에서 작동될 수 있는 큰 위험성이 있음
            Add(() =>
            {
                // 방에 있는 유저들 broadcast
                CUserLeftRoom userLeftRoomMsg = new CUserLeftRoom()
                {
                    LeftUser = leftUser,
                    RoomId = Id,
                };
                Broadcast(userLeftRoomMsg);
            });
        }
    }
}
