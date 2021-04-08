using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityGetScreenshotCommand : AltUnityCommand
    {
        readonly AltClientSocketHandler handler;
        private readonly AltUnityScreenshotReadyCommand getScreenshotCommand;

        public AltUnityGetScreenshotCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 4)
        {
            this.handler = handler;
            getScreenshotCommand = new AltUnityScreenshotReadyCommand(Parameters);
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeTexturedScreenshot(handler, getScreenshotCommand));
            return "Ok";
        }
    }
}
