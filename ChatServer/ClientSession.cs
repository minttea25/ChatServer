using ServerCoreTCP.MessageWrapper;
using System;
using System.Net;

namespace ChatServer
{
    public class ClientSession : PacketSession
    {
        public override void InitSession()
        {
            throw new NotImplementedException();
        }

        public override void OnConnected(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void OnDisconnected(EndPoint endPoint, object error = null)
        {
            throw new NotImplementedException();
        }

        public override void OnRecv(ReadOnlySpan<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override void OnSend(int numOfBytes)
        {
            throw new NotImplementedException();
        }

        public override void PreSessionCleanup()
        {
            throw new NotImplementedException();
        }
    }
}
