using System.Net.Sockets;

namespace wm
{
    public struct Client
    {
        public static Client NullClient = new Client { name = "", clientSocket = new Socket(SocketType.Unknown, ProtocolType.Unknown) };

        public string name;
        public Socket clientSocket;

        public readonly bool IsNull()
        {
            return this.name == NullClient.name && this.clientSocket == NullClient.clientSocket;
        }

        public readonly void Close()
        {
            if (!clientSocket.Connected) return;
          
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();  
        }
    }
}
