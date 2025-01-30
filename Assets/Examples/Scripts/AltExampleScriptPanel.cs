/*
    Copyright(C) 2025 Altom Consulting

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

public class AltExampleScriptPanel : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Color normalColor;
    public Color highlightColor;

    public Image sourceImage;
    void Start()
    {
        sourceImage = gameObject.GetComponent<Image>();

        normalColor = sourceImage.color;
        highlightColor = normalColor;
    }

    void Update()
    {
        transform.up = -(Input.acceleration.normalized);
    }

    public void OnPointerDown(PointerEventData data)
    {
        highlightColor = Color.yellow;
        sourceImage.color = highlightColor;
    }

    public void OnPointerUp(PointerEventData data)
    {

        highlightColor = normalColor;
        sourceImage.color = normalColor;
    }

}
