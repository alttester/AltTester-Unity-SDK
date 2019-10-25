using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class EnableDebuggingCommand : Command
    {
        bool activateDebug;

        public EnableDebuggingCommand(bool activateDebug)
        {
            this.activateDebug = activateDebug;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.debugOn = activateDebug;
            return "Ok";
        }
    }
}
