using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AltTester.AltTesterUnitySDK.UI
{
    public class MessageClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Text textComponent;
        private AltDialog dialog;
        private Canvas canvas;
        public void Awake()
        {
            textComponent = GetComponent<Text>();
            dialog = GetComponentInParent<AltDialog>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 localPoint;

            Camera camera = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                textComponent.rectTransform,
                eventData.position,
                camera,
                out localPoint
            );
            if (localPoint.y > 0 && dialog.StillDisplayingNewVersionMessage)
            {
                dialog.DownloadNewVersion();
            }
            else
            {
                dialog.CloseNewVersionMessage(true);
            }
        }
    }
}
