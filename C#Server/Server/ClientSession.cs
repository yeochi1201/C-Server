using ServerCore;
using System;
using System.Net;
using System.Text;
using System.Threading;
using Packet;

namespace Server
{

    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Connected : {endPoint}");
            /*
            Packet packet = new Packet() { packetId = 10, size = 100 }; ;

            ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            byte[] buffer = BitConverter.GetBytes(packet.packetId);
            byte[] buffer2 = BitConverter.GetBytes(packet.size);
            Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset, buffer2.Length);
            Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);


            Send(sendBuff);*/

            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"On Disconnected : {endPoint}");
        }

        public override void OnRecievePacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    {
                        PlayerInfoReq p = new PlayerInfoReq();
                        p.Read(buffer);
                        Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.playerName}");

                        foreach(PlayerInfoReq.Skill skill in p.skills)
                        {
                            Console.WriteLine($"Skill : {skill.skillId}  {skill.level}  {skill.duration}");
                        }
                    }
                    break;
            }

            Console.WriteLine($"[Receive Packet ID : {id} Size : {size}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
