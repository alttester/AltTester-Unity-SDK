using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetScreenshot: Command
    {
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;

        public GetScreenshot(Vector2 size, AltClientSocketHandler handler)
        {
            this.size = size;
            this.handler = handler;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getScreenshot" + size);
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeScreenshot(size, handler));
            return "Ok";
        }
    }
}
