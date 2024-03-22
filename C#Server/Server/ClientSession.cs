using ServerCore;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{
    // server -> client
    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Connected : {endPoint}");
            
            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Disconnected : {endPoint}");
        }

        public override void OnRecievePacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
