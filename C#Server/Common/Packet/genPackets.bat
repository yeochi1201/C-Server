START ../../PacketGenerator/bin/Debug/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y Packets.cs "../../DummyClient/Packet"
XCOPY /Y ClientPacketManager.cs "../../DummyClient/Packet"

XCOPY /Y Packets.cs "../../Server/Packet"
XCOPY /Y ServerPacketManager.cs "../../Server/Packet"