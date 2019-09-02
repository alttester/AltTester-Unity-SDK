using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class UnknowString :  Command
    {
        public override string Execute()
        {
            return AltUnityRunner._altUnityRunner.errorUnknownError;
        }
    }
}
