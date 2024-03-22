using ServerCore;


class PacketHandler
{
    public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        PlayerInfoReq p = packet as PlayerInfoReq;
        Console.WriteLine($"PlayerInfoReq : {p.playerId} {p.playerName}");

        foreach(PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"Skill : {skill.skillId} {skill.duration} {skill.level}");
        }
    }
}