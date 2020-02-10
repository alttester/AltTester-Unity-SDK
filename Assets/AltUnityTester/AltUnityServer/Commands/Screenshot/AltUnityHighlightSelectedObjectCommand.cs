using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHighlightSelectedObjectCommand :AltUnityCommand
    {
        int id;
        string ColorAndWidth;
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;




        public AltUnityHighlightSelectedObjectCommand (int id, string colorAndWidth, Vector2 size, AltClientSocketHandler handler)
        {
            this.id = id;
            ColorAndWidth = colorAndWidth;
            this.size = size;
            this.handler = handler;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("HightlightObject wiht id: " + id);
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
                var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
                float red = float.Parse(piecesColor[0]);
                float green = float.Parse(piecesColor[1]);
                float blue = float.Parse(piecesColor[2]);
                float alpha = float.Parse(piecesColor[3]);

                UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
                float width = float.Parse(pieces[1]);
                var gameObject = AltUnityRunner.GetGameObject(id);

                if (gameObject != null)
                {
                    AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(gameObject, color, width, size, handler));
                }
                else
                    new AltUnityGetScreenshotCommand (size, handler).Execute();
            return "Ok";
        }
    }
}
