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

        public ClientSession CreateNewSession()
        {
            return null;
        }

        public ClientSession Get(uint id)
        {
            return null;
        }

        public bool Remove(uint id)
        {
            return false;
        }

        public bool Remove(ClientSession session)
        {
            return false;
        }
    }
}
