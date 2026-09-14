
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace wm
{
    public class Server
    {

        private Socket _serverSocket;

        private List<Client> _clients = new List<Client>();
        private List<Command> _commands = new List<Command>();

        private bool _isRunning = false;

        private readonly Thread _acceptThread;
        private readonly Thread _receiveThread;
        private readonly Thread _serverInputThread;

        public Server()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 5000);
            _serverSocket.Bind(ipEndPoint);

            _acceptThread = new Thread(HandleAcceptClient);
            _receiveThread = new Thread(HandleReceiveMessage);
            _serverInputThread = new Thread(HandleServerInput);
        }

        public void Start(int clientCount)
        {
            _serverSocket.Listen(clientCount);
            _isRunning = true;

            _acceptThread.Start();
            _receiveThread.Start();
            _serverInputThread.Start();

            while(_isRunning)
            {
                if (!_acceptThread.IsAlive || !_receiveThread.IsAlive || !_serverInputThread.IsAlive)
                {
                    _isRunning = false;
                    CloseServer();
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            CloseServer();
        }

        private Client AcceptClient()
        {
            Socket socket = _serverSocket.Accept();
            Client client = new Client();
            client.clientSocket = socket;

            byte[] buffer = new byte[1024];
            client.clientSocket.Receive(buffer);

            client.name = Encoding.UTF8.GetString(buffer);
            _clients.Add(client);

            return client;
        }

        public void RegisterCommand(Command command)
        {
            _commands.Add(command);
        }

        private void ReceiveMessage(Client client)
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = client.clientSocket.Receive(buffer);
            string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            Console.WriteLine($"{client.name}: {message}");
        }

        public void SendMessage(Client client, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            client.clientSocket.Send(buffer);
        }

        public void BroadCastMessage(string message)
        {
            foreach (Client client in _clients)
                client.clientSocket.Send(Encoding.UTF8.GetBytes(message));
        }

        public Client GetClient(string name)
        {
            foreach(Client client in _clients)
            { 
                if (client.name == name)
                    return client;
            }

            return Client.NullClient;
        }

        private void CloseClient(Client client)
        {
            client.clientSocket.Shutdown(SocketShutdown.Both);
            client.clientSocket.Close();
            _clients.Remove(client);

            BroadCastMessage($"[Server] {client.name} left the chat !");
        }

        private void CloseServer()
        {
            foreach (Client client in _clients)
            {
                SendMessage(client, "[Server] Server is shutting down !");
                CloseClient(client);
            }

            _serverSocket.Close();
        }

        private void HandleAcceptClient()
        {
            while(_isRunning)
            {
                Client client = AcceptClient();
                BroadCastMessage($"[Server] {client.name} joined the chat !");
            }
        }

        private void HandleReceiveMessage()
        {
            while(_isRunning)
            {
                foreach(Client client in _clients)
                {
                    try
                    {
                        ReceiveMessage(client);
                    } catch(SocketException) {
                        CloseClient(client);
                    }
                }
            }
        }

        private void HandleServerInput()
        {
            while(_isRunning)
            {
                string? cmd = Console.ReadLine();
                if (cmd == null) continue;

                string[] args = cmd.Split(' ');
                Command? command = _commands.Find(c => c.CallCommand == args[0]);
                command?.Execute(this, args[1..]);
            }
        }
    }
}