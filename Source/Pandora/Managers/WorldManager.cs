using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pandora.Network;

namespace Pandora.Managers
{
    public static class WorldManager
    {
        private static readonly List<Session> sessions = new List<Session>();
        private static readonly object mutex = new object();

        private static Thread worldThread;

        public static void Initialise()
        {
            worldThread = new Thread(() =>
            {
                uint lastTick = 0u;
                for (;;)
                {
                    var startTime = DateTime.UtcNow;

                    // TODO: ...

                    Thread.Sleep(1);
                    lastTick = (uint)(DateTime.UtcNow - startTime).TotalMilliseconds;
                }
            });

            worldThread.Start();
        }

        public static void AddSession(Session session)
        {
            lock (mutex)
                sessions.Add(session);
        }

        public static Session GetSession(string username)
        {
            lock (mutex)
                return sessions.FirstOrDefault(session => session?.Account.Username == username);
        }
    }
}
