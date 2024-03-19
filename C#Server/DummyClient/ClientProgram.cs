using ServerCore;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return new ServerSession(); });

            while (true)
            {
                try
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(100);
            }
        }
    }
}
