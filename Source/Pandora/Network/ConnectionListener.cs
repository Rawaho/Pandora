using System.Net;
using System.Net.Sockets;
using Pandora.Managers;

namespace Pandora.Network
{
    public class ConnectionListener
    {
        private readonly ServerType type;
        private readonly TcpListener listener;

        public ConnectionListener(uint port, ServerType serverType)
        {
            type     = serverType;
            listener = new TcpListener(IPAddress.Any, (int)port);
            listener.Start();
        }

        public async void AcceptConnection()
        {
            if (listener.Pending())
                WorldManager.AddSession(new Session(await listener.AcceptSocketAsync(), type));
        }
    }
}
