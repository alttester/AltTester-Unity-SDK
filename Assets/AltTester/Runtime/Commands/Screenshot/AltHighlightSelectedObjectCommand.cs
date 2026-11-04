/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltHighlightSelectedObjectCommand : AltBaseScreenshotCommand<AltHighlightObjectScreenshotParams, string>
    {

        public AltHighlightSelectedObjectCommand(ICommandHandler handler, AltHighlightObjectScreenshotParams cmdParams) : base(handler, cmdParams)
        {

        }

        public override string Execute()
        {
            var gameObject = AltRunner.GetGameObject(CommandParams.altObjectId, false);
            AltObject altGameObject = null;
            if (gameObject != null)
            {
                altGameObject = AltRunner._altRunner.GameObjectToAltObject(gameObject);
            }

            if (altGameObject != null)
            {
                var color = new UnityEngine.Color(CommandParams.color.r, CommandParams.color.g, CommandParams.color.b, CommandParams.color.a);

                AltRunner._altRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality, altGameObject, color, CommandParams.width));
            }
            else
            {
                AltRunner._altRunner.StartCoroutine(SendTexturedScreenshotCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality));
            }
            return "Ok";
        }
    }
}
