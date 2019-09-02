using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class EnableDebugging : Command
    {
        bool activateDebug;

        public EnableDebugging(bool activateDebug)
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
