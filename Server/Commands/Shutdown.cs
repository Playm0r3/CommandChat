using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm
{
    public class Shutdown : Command
    {

        public Shutdown()
        {
            Name = "Shutdown";
            Description = "Shuts down the server";
            Usage = "shutdown-server";
            CallCommand = "shutdown-server";
        }

        public override void Execute(Server server, string[] args)
        {
            server.BroadCastMessage("[Server] Server is shutting down...");
            server.Stop();
        }
    }
}
