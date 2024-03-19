using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using ServerCore;

namespace Server
{
    class ServerProgram
    {
        static Listener _listner = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                ClientSession session = new ClientSession();
                session.Start(clientSocket);

                Thread.Sleep(1000);

                session.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //Listen Socket
            _listner.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening");
            while (true)
            {
                ;
            }
        }
    }
}
