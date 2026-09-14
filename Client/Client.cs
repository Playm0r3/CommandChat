using System.Net;
using System.Net.Sockets;
using System.Text;

namespace wm
{
    public class Client
    {

        private readonly Socket clientSocket;
        private readonly string name;

        private bool _isRunning = false;

        private readonly Thread _inputThread;
        private readonly Thread _outputThread;

        public Client(string name)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.name = name;

            _inputThread = new Thread(HandleUserInput);
            _outputThread = new Thread(HandleReceiving);
        }

        public void Connect(string addresse, int port)
        {
            clientSocket.Connect(addresse, port);
            Console.WriteLine($"[System] Connected to server on port {port}");
            _isRunning = true;

            SendMessage(name);
        }

        public void StartClient()
        {
            _inputThread.Start();
            _outputThread.Start();
        }

        private void SendMessage(string? message)
        {
            if (message == null) return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            clientSocket.Receive(buffer);
            string message = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(message);
        }

        private void Disconnect()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            _isRunning = false;

            Console.WriteLine("[System] Disconnected from server");
        }

        private void HandleUserInput()
        {
            while (_isRunning)
            {
                string? input = Console.ReadLine();
                if (input?.ToLower() == "exit-client")
                {
                    Disconnect();
                    break;
                }

                SendMessage(input);
            }
        }

        private void HandleReceiving()
        {
            while (_isRunning)
            {
                ReceiveMessages();
            }
        }

    }
}
