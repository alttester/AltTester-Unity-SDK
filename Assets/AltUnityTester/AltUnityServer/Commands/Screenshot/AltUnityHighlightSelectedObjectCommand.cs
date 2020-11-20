using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHighlightSelectedObjectCommand : AltUnityCommand
    {
        int id;
        string ColorAndWidth;
        UnityEngine.Vector2 size;
        int quality;
        AltClientSocketHandler handler;

        public AltUnityHighlightSelectedObjectCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 6)
        {
            this.handler = handler;
            this.id = System.Convert.ToInt32(parameters[2]);
            ColorAndWidth = parameters[3];
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[4]);
            this.quality = Int32.Parse(parameters[5]);
        }

        public override string Execute()
        {
            LogMessage("HightlightObject wiht id: " + id);
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);
            var gameObject = AltUnityRunner.GetGameObject(id);

            var getScreenshotCommand = new AltUnityGetScreenshotCommand(handler, Parameters[0], Parameters[1], Parameters[4], Parameters[5]);
            if (gameObject != null)
            {
                AltUnityRunner._altUnityRunner.StartCoroutine(
                    AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(gameObject, color, width, getScreenshotCommand));
            }
            else
                getScreenshotCommand.Execute();
            return "Ok";
        }
    }
}
