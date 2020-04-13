using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOM
{
    public abstract class Command
    {
        public Command() { }

        public abstract string CommandName { get; set; }

        public abstract Task Run();
    }
}
