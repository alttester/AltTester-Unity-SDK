using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetScreenshotCommand : AltUnityCommand
    {
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;
        int quality;

        public AltUnityGetScreenshotCommand (Vector2 size, int quality, AltClientSocketHandler handler)
        {
            this.size = size;
            this.handler = handler;
            this.quality = quality;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getScreenshot" + size);
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeTexturedScreenshot(size,quality, handler));
            return "Ok";
        }
    }
}
