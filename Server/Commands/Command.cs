using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wm
{
    public abstract class Command
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Usage { get; set; } = "";
        public string CallCommand { get; set; } = "";
        public abstract void Execute(Server server, string[] args);
    }
}
