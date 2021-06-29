using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltUnityExampleScriptIncrementOnClick : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public Text counterText;
    int counter = 0;

    int keyPressDownCounter = 0;
    int keyPressUpCounter = 0;

    private int mouseDownCounter = 0;
    private int mouseUpCounter = 0;
    private int mousePressedCounter = 0;
    private string eventDataPressRaycastObject = "";
    private HashSet<string> eventsRaised = new HashSet<string>();

    void Start()
    {
        counter = 0;
        counterText.text = counter.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keyPressDownCounter++;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            keyPressUpCounter++;
        }

        if (Input.GetMouseButton(0))
        {
            mousePressedCounter++;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownCounter++;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpCounter++;
        }
    }
    public void ButtonPressed()
    {
        counter++;
        counterText.text = counter.ToString();
    }

    protected void OnMouseEnter()
    {
        eventsRaised.Add("OnMouseEnter");
    }
    protected void OnMouseDown()
    {
        eventsRaised.Add("OnMouseDown");
    }
    protected void OnMouseUp()
    {
        eventsRaised.Add("OnMouseUp");
    }

    protected void OnMouseUpAsButton()
    {
        eventsRaised.Add("OnMouseUpAsButton");
    }
    protected void OnMouseExit()
    {
        eventsRaised.Add("OnMouseExit");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        eventDataPressRaycastObject = eventData.pointerPressRaycast.gameObject.name;
    }
}
