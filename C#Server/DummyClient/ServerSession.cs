using ServerCore;
using System;
using System.Net;
using System.Text;
using Packet;

namespace DummyClient
{
    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Connected : {endPoint}");
            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, playerName="ABCD"};
            packet.skills.Add(new PlayerInfoReq.Skill() { skillId = 101, level = 1, duration = 3.0f });
            packet.skills.Add(new PlayerInfoReq.Skill() { skillId = 102, level = 2, duration = 4.0f });
            packet.skills.Add(new PlayerInfoReq.Skill() { skillId = 103, level = 3, duration = 5.0f });
            packet.skills.Add(new PlayerInfoReq.Skill() { skillId = 104, level = 4, duration = 6.0f });
            
            ArraySegment<byte> s = packet.Write();
            if (s != null)
            {
                Send(s);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Disconnected : {endPoint}");
        }

        public override int OnReceive(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
