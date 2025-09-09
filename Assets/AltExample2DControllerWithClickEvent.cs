using UnityEngine;
using UnityEngine.EventSystems;
#if TMP_PRESENT
using TMPro;
#endif

public class AltExample2DControllerWithClickEvent : MonoBehaviour, IPointerClickHandler
{
#if TMP_PRESENT
    private TMPro.TMP_Text text;
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
#if TMP_PRESENT

        text.text = $"Clicked on {gameObject.name}";
#endif
    }

#if TMP_PRESENT

    void Awake() => text = GameObject.Find("Text").GetComponent<TMPro.TMP_Text>();
#endif
}
