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

using System.Collections;
using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AltTester.AltTesterUnitySDK.UI
{

    public class AltPrefabDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var group = eventData.pointerDrag.AddComponent<CanvasGroup>();
                group.blocksRaycasts = false;

            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.transform.position = InputMisc.GetMousePosition();
                var objectTranform = (RectTransform)eventData.pointerDrag.transform;
                if (objectTranform.position.x < objectTranform.rect.width)
                {
                    objectTranform.position = new Vector3(objectTranform.rect.width, objectTranform.position.y, objectTranform.position.z);
                }
                else if (objectTranform.position.x > Screen.width)
                {
                    objectTranform.position = new Vector3(Screen.width, objectTranform.position.y, objectTranform.position.z);
                }
                if (objectTranform.position.y < 0)
                {
                    objectTranform.position = new Vector3(objectTranform.position.x, 0, objectTranform.position.z);
                }
                else if (objectTranform.position.y > Screen.height - objectTranform.rect.height)
                {
                    objectTranform.position = new Vector3(objectTranform.position.x, Screen.height - objectTranform.rect.height, objectTranform.position.z);
                }

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var canvasGroup = eventData.pointerDrag.GetComponent<CanvasGroup>();
                Destroy(canvasGroup);
            }
        }

    }

}
