using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetScreenshotCommand : AltUnityCommand
    {
        AltClientSocketHandler handler;
        UnityEngine.Vector2 size;
        int quality;

        public AltUnityGetScreenshotCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 4)
        {
            this.handler = handler;
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.quality = Int32.Parse(parameters[3]);
        }

        public override string Execute()
        {
            LogMessage("getScreenshot" + size);
            var getScreenshotCommand = new AltUnityScreenshotReadyCommand(Parameters);
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeTexturedScreenshot(handler, getScreenshotCommand));
            return "Ok";
        }
    }
}
