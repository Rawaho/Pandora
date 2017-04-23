namespace Pandora.Network.Commands
{
    public abstract class ClientCommand
    {
        public abstract void Handle(Session session);
    }
}
