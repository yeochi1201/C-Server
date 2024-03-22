﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class SessionManager
    {
        static SessionManager _instance = new SessionManager();
        public static SessionManager Instance { get {  return _instance; } }

        static int _sessionId = 0;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        object _lock = new object();

        public ClientSession Generate()
        {
            lock (_lock)
            {
                int sessionId = _sessionId++;

                ClientSession session = new ClientSession();
                session.SessionId = sessionId++;
                _sessions.Add(sessionId, session);

                Console.WriteLine($"Session Connected : {sessionId}");

                return session;
            }
        }

        public ClientSession Find(int sessionId)
        {
            lock (_lock)
            {
                ClientSession session = null;
                _sessions.TryGetValue(sessionId, out session);
                return session;
            }
        }

        public void Remove(ClientSession session)
        {
            lock ( _lock)
            {
                _sessions.Remove(session.SessionId);
            }
        }
    }
}
