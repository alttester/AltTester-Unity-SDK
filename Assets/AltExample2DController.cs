using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AltExample2DController : MonoBehaviour, IPointerClickHandler
{
    private TMPro.TMP_Text text;

    public void OnPointerClick(PointerEventData eventData)
    {
        text.text = $"Clicked on {gameObject.name}";
    }

    void Awake() => text = GameObject.Find("Text").GetComponent<TMPro.TMP_Text>();

}
