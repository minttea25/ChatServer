using System;
using System.Collections.Generic;

namespace ChatServer
{
    public class SessionManager
    {
        #region Singleton
        readonly static SessionManager _instance = new SessionManager();
        public static SessionManager Instance => _instance;
        #endregion

        uint _sessionId = 0;
        readonly Dictionary<uint, ClientSession> _sessions = new Dictionary<uint, ClientSession>();
        readonly object _lock = new object();

        public ClientSession CreateNewSession()
        {
            lock (_lock)
            {
                uint id = _sessionId++;
                ClientSession session = new ClientSession(id);
                _sessions.Add(id, session);

                return session;
            }
        }

        public ClientSession Get(uint id)
        {
            lock (_lock)
            {
                if (_sessions.TryGetValue(id, out var session))
                {
                    return session;
                }
                else return null;
            }
        }

        public bool Remove(uint id)
        {
            lock (_lock)
            {
                return _sessions.Remove(id);
            }
        }

        public bool Remove(ClientSession session)
        {
            var rooms = session.Rooms.Values;
            foreach (var r in rooms)
            {
                r.Leave(session);
            }

            lock (_lock)
            {
                return _sessions.Remove(session.SessionId);
            }
        }
    }
}
