/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltGetApplicationScreenSizeCommand : AltCommand<AltGetApplicationScreenSizeParams, AltVector2>
    {
        public AltGetApplicationScreenSizeCommand(AltGetApplicationScreenSizeParams cmdParams) : base(cmdParams) { }

        public override AltVector2 Execute()
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            return new AltVector2(screenWidth, screenHeight);
        }
    }
}
