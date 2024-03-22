using System;
using System.Net;
using System.Net.Sockets;


namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;
        int maxClient = 10;


        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            _sessionFactory = sessionFactory;
            //socket option setting
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(endPoint);

            //socket waiting (max client)
            _listenSocket.Listen(maxClient);

            //socket asynchronous event setting
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            //socket accepting true => continue accepting / false => success
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false) 
            {
                //accepting complete event start
                OnAcceptCompleted(null, args);
            }
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                //session create
                Session session = _sessionFactory.Invoke();
                //session start
                session.Start(args.AcceptSocket);
                //session connect event
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine(args.SocketError);
            }
            RegisterAccept(args);
        }

        public Socket Accept()
        {
            return _listenSocket.Accept();
        }
    }
}
