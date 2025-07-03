using System.Text.RegularExpressions;
using AltTester.AltTesterUnitySDK.InputModule;
using TMPro; // Remove this if you're not using TextMeshPro
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltCopyTextOnClick : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI TmpText;

    public void OnPointerClick(PointerEventData eventData)
    {
        AltConsoleLogViewer.Instance.ShowClipboardNotification(InputMisc.GetMousePosition());

#if UNITY_WEBGL && !UNITY_EDITOR
    AltConsoleLogViewer.Instance.Copy(Regex.Replace(TmpText.text, "<.*?>", string.Empty));
#else
        GUIUtility.systemCopyBuffer = Regex.Replace(TmpText.text, "<.*?>", string.Empty);
#endif
    }
}
