using System.Net.Sockets;

namespace wm
{
    public abstract class Protocol
    {

        public static Protocol CreateProtocol(ProtocolType protocolType)
        {
            return protocolType switch
            {
                ProtocolType.Message => new Message(),
                _ => throw new ArgumentException("Invalid protocol type"),
            };
        }

    }
}
