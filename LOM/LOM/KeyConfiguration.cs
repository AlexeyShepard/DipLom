using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOM
{
    public static class KeyConfiguration
    {
        public static Command[] List = new Command[] { new SilentCommand(),
                                                       new PinCommand(),
                                                       new LoadCommand(),
                                                       new StopCommand()};
    }
}
