using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace wm
{
    public class Message : Protocol
    {
        public Message() { }

        public byte[] Encode(string message)
        {
            byte[] payload = Encoding.UTF8.GetBytes(message);
            byte[] messageLength = BitConverter.GetBytes(payload.Length);

            byte[] packet = new byte[4 + payload.Length];

            Buffer.BlockCopy(messageLength, 0, packet, 0, 4);
            Buffer.BlockCopy(payload, 0, packet, 4, payload.Length);

            return packet;
        }

        public string Decode(byte[] packet)
        {
            int messageLength = BitConverter.ToInt32(packet, 0);
            byte[] payload = new byte[messageLength];
            Buffer.BlockCopy(packet, 4, payload, 0, messageLength);
            return Encoding.UTF8.GetString(payload);
        }
    
        public void SendMessage(Socket socket, string message)
        {
            byte[] packet = Encode(message);
            int bytesSent = 0;

            while(bytesSent < packet.Length)
                bytesSent += socket.Send(packet, bytesSent, packet.Length - bytesSent, SocketFlags.None);
        }

        // Faire une fonction pour recevoir un certain nombre de bytes 

        // Faire une fonction pour recevoir un message complet
    }
}
