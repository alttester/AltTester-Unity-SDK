using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetScreenshotCommand : AltUnityBaseScreenshotCommand
    {
        UnityEngine.Vector2 size;
        readonly int quality;

        public AltUnityGetScreenshotCommand(AltClientSocketHandler handler, params string[] parameters) : base(handler, parameters, 4)
        {
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.quality = JsonConvert.DeserializeObject<int>(parameters[3]);
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(size, quality));
            return "Ok";
        }
    }
}
