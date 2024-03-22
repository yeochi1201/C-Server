using System;
using System.Xml;

namespace PacketGenerator
{
    class Program
    {
        static string genPackets="";
        static string packetEnums = "";
        static ushort packetId = 0;

        static string clientRegister;
        static string serverRegister;

        static void Main(string[] args)
        {
            //PDL location
            string pdlPath = "../../PDL.xml";

            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            if(args.Length >=1 ) 
            {
                pdlPath = args[0];
            }

            using (XmlReader r = XmlReader.Create(pdlPath, settings))
            {
                r.MoveToContent();

                while(r.Read())
                {
                    //packet parsing
                    if(r.Depth== 1 && r.NodeType == XmlNodeType.Element)
                    {
                        ParsePacket(r);
                    }
                }
                //create packet file
                string fileText = string.Format(PacketFormat.fileFormat, packetEnums, genPackets);
                File.WriteAllText("Packets.cs", fileText);
                string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister);
                File.WriteAllText("ServerPacketManager.cs", serverManagerText);
                string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister);
                File.WriteAllText("ClientPacketManager.cs", clientManagerText);
            }
        }

        public static void ParsePacket(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.EndElement)
            {
                return;
            }
            if (r.Name.ToLower() != "packet")
            {
                Console.WriteLine("Invaild Packet Node");
                return;
            }

            string packetName = r["name"];
            if(string.IsNullOrEmpty(packetName) )
            {
                Console.WriteLine("Packet Without Name");
                return;
            }
            //packet class attribute parsing
            Tuple<string, string, string> t = ParseMembers(r);
            //packet header format
            genPackets += string.Format(PacketFormat.packetFormat, packetName,t.Item1,t.Item2,t.Item3);
            //packetId enum format
            packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, ++packetId)+ "\t";
            //packet manager register format
            if(packetName.StartsWith("S_") || packetName.StartsWith("s_"))
            {
                clientRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
            }
            if (packetName.StartsWith("C_") || packetName.StartsWith("c_"))
            {
                serverRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
            }
            else
            {
                clientRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
                serverRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
            }
        }
        //general type attribute parsing
        public static Tuple<string, string, string> ParseMembers(XmlReader r)
        {
            string pakcetName = r["name"];
            string memberCode = "";
            string readCode = "";
            string writeCode = "";

            int depth = r.Depth + 1;
            while(r.Read() )
            {
                if(r.Depth != depth)
                {
                    break;
                }

                string memberName = r["name"];
                if (string.IsNullOrEmpty(memberName))
                {
                    Console.WriteLine("Member Without Name");
                    return null;
                }

                if(string.IsNullOrEmpty (memberCode) == false)
                {
                    memberCode += Environment.NewLine;
                }
                if (string.IsNullOrEmpty(readCode) == false)
                {
                    readCode += Environment.NewLine;
                }
                if (string.IsNullOrEmpty(writeCode) == false)
                {
                    writeCode += Environment.NewLine;
                }

                string memberType = r.Name.ToLower();
                switch(memberType)
                {
                    case "byte":
                    case "sbyte":
                        memberCode += string.Format(PacketFormat.memberForamt, memberType, memberName);
                        readCode += string.Format(PacketFormat.readByteFormat, memberType, memberName);
                        writeCode += string.Format(PacketFormat.writeByteFormat, memberType, memberName);
                        break;
                    case "bool":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                        memberCode += string.Format(PacketFormat.memberForamt, memberType, memberName);
                        readCode += string.Format(PacketFormat.readFormat, memberType, memberName, ToMemberType(memberType));
                        writeCode += string.Format(PacketFormat.writeFormat, memberType, memberName);
                        break;
                    case "string":
                        memberCode += string.Format(PacketFormat.memberForamt, memberType, memberName);
                        readCode += string.Format(PacketFormat.readStringFormat, memberName);
                        writeCode += string.Format(PacketFormat.writeStringFormat, memberName);
                        break;
                    case "list":
                        Tuple<string, string, string> t = ParseList(r);
                        memberCode += t.Item1;
                        readCode += t.Item2;
                        writeCode += t.Item3;
                        break;
                    default:
                        break;
                }
            }
            memberCode = memberCode.Replace("\n", "\n\t");
            readCode = readCode.Replace("\n", "\n\t\t");
            writeCode = writeCode.Replace("\n", "\n\t\t");
            return new Tuple<string, string, string>(memberCode,readCode,writeCode);
        }
        //attribute list parsing
        public static Tuple<string, string, string> ParseList(XmlReader r)
        {
            string listName = r["name"];
            if (string.IsNullOrEmpty(listName))
            {
                Console.WriteLine("List Without Name");
                return null;
            }

            Tuple<string, string, string> t = ParseMembers(r);
            string memberCode = string.Format(PacketFormat.memberListFormat, FirstCharToUpper(listName),FirstCharToLower(listName),t.Item1, t.Item2, t.Item3);
            string readCode = string.Format(PacketFormat.readListFormat, FirstCharToUpper(listName), FirstCharToLower(listName));
            string writeCode = string.Format(PacketFormat.writeListFormat, FirstCharToUpper(listName), FirstCharToLower(listName));

            return new Tuple<string, string, string>(memberCode,readCode ,writeCode);
        }
        //ToMemberType function parsing
        public static string ToMemberType(string memberType)
        {
            switch (memberType)
            {
                case "bool":
                    return "ToBoolean";
                case "short":
                    return "ToInt16";
                case "ushort":
                    return "ToUInt16";
                case "int":
                    return "ToInt32";
                case "long":
                    return "ToInt64";
                case "float":
                    return "ToSingle";
                case "double":
                    return "ToDouble";
                default:
                    return "";
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return "";
            }
            return input[0].ToString().ToUpper()+input.Substring(1);
        }

        public static string FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return "";
            }
            return input[0].ToString().ToLower() + input.Substring(1);
        }
    }
}