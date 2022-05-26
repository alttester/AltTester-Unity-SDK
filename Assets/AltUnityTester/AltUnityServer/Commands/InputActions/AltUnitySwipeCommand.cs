using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnitySwipeCommand : AltUnityCommandWithWait<AltUnitySwipeParams, string>
    {
        public AltUnitySwipeCommand(ICommandHandler handler, AltUnitySwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            
            UnityEngine.Vector2[] positions = { CommandParams.start.ToUnity(), CommandParams.end.ToUnity() };
            InputController.SetMultipointSwipe(positions,CommandParams.duration,onFinish);
            return "Ok";
        }
    }
}
