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
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Connected : {endPoint}");

            ServerProgram.room.Push(() => ServerProgram.room.Enter(this));
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);
            if(Room!= null)
            {
                GameRoom room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }
            Console.WriteLine($"On Disconnected : {endPoint}");
        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
