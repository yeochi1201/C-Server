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
            //DNS setting
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            //client socket create
            Connector connector = new Connector();
            //session create
            connector.Connect(endPoint, () => { return SessionManager.Instance.Generate(); }, 100);

            while (true)
            {
                try
                {
                    SessionManager.Instance.SendForEach();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(250);
            }
        }
    }
}
