using System.Net;
using ServerCore;

namespace Server
{
    class ServerProgram
    {
        //server socket create
        static Listener _listner = new Listener();
        public static GameRoom room = new GameRoom();

        static void FlushRoom()
        {
            room.Push(() => room.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        static void Main(string[] args)
        {
            //DNS setting
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //Listen Socket
            _listner.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening");

            FlushRoom();

            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}
