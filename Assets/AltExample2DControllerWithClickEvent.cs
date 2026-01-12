using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class AltExample2DControllerWithClickEvent : MonoBehaviour, IPointerClickHandler
{
    private Text text;

    public void OnPointerClick(PointerEventData eventData)
    {

        text.text = $"Clicked on {gameObject.name}";
    }


    void Awake() => text = GameObject.Find("Text").GetComponent<Text>();
}
