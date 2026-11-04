/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Linq;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltMultipointSwipeCommand : AltCommandWithWait<AltMultipointSwipeParams, string>
    {
        public AltMultipointSwipeCommand(ICommandHandler handler, AltMultipointSwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.SetMultipointSwipe(CommandParams.positions.Select(p => new UnityEngine.Vector2(p.x, p.y)).ToArray(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
