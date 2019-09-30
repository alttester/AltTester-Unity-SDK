using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetScreenshotCommand: Command
    {
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;

        public GetScreenshotCommand(Vector2 size, AltClientSocketHandler handler)
        {
            this.size = size;
            this.handler = handler;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("getScreenshot" + size);
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeScreenshot(size, handler));
            return "Ok";
        }
    }
}
