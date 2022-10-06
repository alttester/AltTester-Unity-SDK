using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltExampleScriptDropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image containerImage;
    public Image receivingImage;
    private Color normalColor;
    public Color highlightColor;

    public void OnEnable()
    {
        if (containerImage != null)
        {
            normalColor = containerImage.color;
            highlightColor = normalColor;
        }
    }

    public void OnDrop(PointerEventData data)
    {
        containerImage.color = normalColor;

        if (receivingImage == null)
            return;

        Sprite dropSprite = GetDropSprite(data);
        if (dropSprite != null)
            receivingImage.overrideSprite = dropSprite;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (containerImage == null)
            return;

        Sprite dropSprite = GetDropSprite(data);
        highlightColor = Color.yellow;
        containerImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (containerImage == null)
            return;
        highlightColor = normalColor;
        containerImage.color = normalColor;
    }

    private Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<AltExampleScriptDragMe>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        return srcImage.sprite;
    }
}
