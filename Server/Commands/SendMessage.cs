using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm
{
    internal class SendMessage : Command
    {

        public SendMessage()
        {
            Name = "Send";
            Description = "Sends a message to clients";
            Usage = "send-message [ClientName] [Message]";
            CallCommand = "send-message";
        }

        public override void Execute(Server server,string[] args)
        {
            Client client = server.GetClient(args[0]);
            if (client.IsNull()) return;

            string message = string.Join("", args.Skip(1));
            server.SendMessage(client, message);
        }
    }
}
