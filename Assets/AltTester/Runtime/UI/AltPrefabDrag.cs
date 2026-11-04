/*
    Copyright(C) 2026 Altom Consulting
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
