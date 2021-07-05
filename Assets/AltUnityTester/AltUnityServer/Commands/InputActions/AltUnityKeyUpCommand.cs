using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityKeyUpCommand : AltUnityCommand
    {
#if ALTUNITYTESTER
        readonly KeyCode keyCode;
#endif      

        public AltUnityKeyUpCommand(params string[] parameters) : base(parameters, 3)
        {
#if ALTUNITYTESTER
            keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), parameters[2]);
#endif
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.KeyUp(keyCode);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
