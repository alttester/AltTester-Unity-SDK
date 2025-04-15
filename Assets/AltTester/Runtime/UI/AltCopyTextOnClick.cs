using System.Text.RegularExpressions;
using TMPro; // Remove this if you're not using TextMeshPro
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltCopyTextOnClick : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI TmpText;

    public void OnPointerClick(PointerEventData eventData)
    {
        AltConsoleLogViewer.Instance.ShowClipboardNotification(Input.mousePosition);
        GUIUtility.systemCopyBuffer = Regex.Replace(TmpText.text, "<.*?>", string.Empty);
    }
}
