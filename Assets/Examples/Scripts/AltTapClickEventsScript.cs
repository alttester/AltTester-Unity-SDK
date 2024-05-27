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

public class AltTapClickEventsScript : MonoBehaviour,
 IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,
 IPointerClickHandler
{
    private HashSet<string> monoBehaviourEventsRaised = new HashSet<string>();
    private HashSet<string> eventSystemEventsRaised = new HashSet<string>();

    protected void OnMouseEnter()
    {
        monoBehaviourEventsRaised.Add("OnMouseEnter");
    }
    protected void OnMouseDown()
    {
        monoBehaviourEventsRaised.Add("OnMouseDown");
    }
    protected void OnMouseUp()
    {
        monoBehaviourEventsRaised.Add("OnMouseUp");
    }

    protected void OnMouseUpAsButton()
    {
        monoBehaviourEventsRaised.Add("OnMouseUpAsButton");
    }
    protected void OnMouseExit()
    {
        monoBehaviourEventsRaised.Add("OnMouseExit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerUp");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerClick");
    }
}
