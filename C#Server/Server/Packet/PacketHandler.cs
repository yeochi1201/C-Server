using Server;
using ServerCore;


class PacketHandler
{
    public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        //TODO
    }

    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;

        if(clientSession.Room == null)
        {
            return;
        }
        GameRoom room = clientSession.Room as GameRoom;
        room.Push(() => room.Broadcast(clientSession, chatPacket.chat));
    }
}