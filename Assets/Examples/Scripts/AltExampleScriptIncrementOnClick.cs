/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltExampleScriptIncrementOnClick : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public Text counterText;
    int counter = 0;

    int keyPressDownCounter = 0;
    int keyPressUpCounter = 0;

    private int mouseDownCounter = 0;
    private int mouseUpCounter = 0;
    private int mousePressedCounter = 0;

    private Vector2 pointerPress;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerPress = eventData.pressPosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.gameObject != null)
            eventDataPressRaycastObject = eventData.pointerPressRaycast.gameObject.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventsRaised.Add("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventsRaised.Add("OnPointerExit");
    }


}
