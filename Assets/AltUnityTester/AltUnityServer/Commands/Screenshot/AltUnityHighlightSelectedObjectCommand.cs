using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHighlightSelectedObjectCommand : AltUnityBaseScreenshotCommand
    {
        readonly int id;
        readonly string colorAndWidth;
        private UnityEngine.Vector2 size;
        readonly int quality;

        public AltUnityHighlightSelectedObjectCommand(AltClientSocketHandler handler, params string[] parameters) : base(handler, parameters, 6)
        {
            this.id = JsonConvert.DeserializeObject<int>(parameters[2]);
            colorAndWidth = parameters[3];
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[4]);
            this.quality = JsonConvert.DeserializeObject<int>(parameters[5]);
        }

        public override string Execute()
        {
            var gameObject = AltUnityRunner.GetGameObject(id);

            if (gameObject != null)
            {
                var pieces = colorAndWidth.Split(new[] { "!-!" }, StringSplitOptions.None);
                var piecesColor = pieces[0].Split(new[] { "!!" }, StringSplitOptions.None);
                float red = float.Parse(piecesColor[0]);
                float green = float.Parse(piecesColor[1]);
                float blue = float.Parse(piecesColor[2]);
                float alpha = float.Parse(piecesColor[3]);

                var color = new UnityEngine.Color(red, green, blue, alpha);
                float width = float.Parse(pieces[1]);

                AltUnityRunner._altUnityRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(size, quality, gameObject, color, width));
            }
            else
            {
                AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(size, quality));
            }
            return "Ok";
        }
    }
}
