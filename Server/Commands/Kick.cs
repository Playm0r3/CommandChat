using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm
{
    public class Kick : Command
    {

        public Kick() 
        { 
            Name = "Kick";
            Description = "Kicks a client from the server";
            Usage = "kick-client [ClientName]";
            CallCommand = "kick-client";
        }

        public override void Execute(Server server, string[] args)
        {
            Client client = server.GetClient(args[0]);
            if (client.IsNull()) return;

            server.SendMessage(client, "[Server] You have been kicked from the server.");
            client.Close();
        }
    }
}
