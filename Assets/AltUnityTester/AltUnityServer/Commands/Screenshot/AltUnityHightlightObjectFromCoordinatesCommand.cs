using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand :AltUnityCommand
    {
        UnityEngine.Vector2 screenCoordinates;
        string ColorAndWidth;
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;


        public AltUnityHightlightObjectFromCoordinatesCommand (Vector2 screenCoordinates, string colorAndWidth, Vector2 size, AltClientSocketHandler handler)
        {
            this.screenCoordinates = screenCoordinates;
            ColorAndWidth = colorAndWidth;
            this.size = size;
            this.handler = handler;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("HightlightObject with coordinates: " + screenCoordinates);
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
                var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
                float red = float.Parse(piecesColor[0]);
                float green = float.Parse(piecesColor[1]);
                float blue = float.Parse(piecesColor[2]);
                float alpha = float.Parse(piecesColor[3]);

                UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
                float width = float.Parse(pieces[1]);

                UnityEngine.Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenCoordinates);
                UnityEngine.RaycastHit[] hits;
                var raycasters = UnityEngine.GameObject.FindObjectsOfType<UnityEngine.UI.GraphicRaycaster>();
                UnityEngine.EventSystems.PointerEventData pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                pointerEventData.position = screenCoordinates;
                foreach (var raycaster in raycasters)
                {
                    System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> hitUI = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                    raycaster.Raycast(pointerEventData, hitUI);
                    foreach (var hit in hitUI)
                    {
                        AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(hit.gameObject, color, width, size, handler));
                    return "Ok";
                    }
                }
                hits = UnityEngine.Physics.RaycastAll(ray);
                if (hits.Length > 0)
                {
                AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(hits[hits.Length - 1].transform.gameObject, color, width, size, handler));
                }
                else
                {
                    new AltUnityGetScreenshotCommand (size, handler).Execute();
                }
            return "Ok";
        }
    }
}
