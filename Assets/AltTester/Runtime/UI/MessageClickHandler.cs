using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AltTester.AltTesterUnitySDK.UI
{
    public class MessageClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text messageText;
        private AltDialog dialog;
        public void Awake()
        {
            messageText = GetComponent<TMP_Text>();
            dialog = GetComponentInParent<AltDialog>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Check if the clicked text is a link
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(messageText, eventData.position, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = messageText.textInfo.linkInfo[linkIndex];
                if (linkInfo.GetLinkID() == "close")
                {
                    Debug.Log("Close link clicked!");
                    dialog.CloseNewVersionMessage(true);
                }
                else if (linkInfo.GetLinkID() == "download")
                {
                    Debug.Log("Download link clicked!");
                    dialog.DownloadNewVersion();
                }
            }
        }
    }
}
