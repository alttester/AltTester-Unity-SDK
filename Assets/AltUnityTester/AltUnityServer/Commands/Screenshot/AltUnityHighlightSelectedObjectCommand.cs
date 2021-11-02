using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityHighlightSelectedObjectCommand : AltUnityBaseScreenshotCommand<AltUnityHightlightObjectScreenshotParams, string>
    {

        public AltUnityHighlightSelectedObjectCommand(ICommandHandler handler, AltUnityHightlightObjectScreenshotParams cmdParams) : base(handler, cmdParams)
        {

        }

        public override string Execute()
        {
            var gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObjectId);

            if (gameObject != null)
            {
                var color = new UnityEngine.Color(CommandParams.color.r, CommandParams.color.g, CommandParams.color.b, CommandParams.color.a);

                AltUnityRunner._altUnityRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(CommandParams.size.ToUnity(), CommandParams.quality, gameObject, color, CommandParams.width));
            }
            else
            {
                AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(CommandParams.size.ToUnity(), CommandParams.quality));
            }
            return "Ok";
        }
    }
}
