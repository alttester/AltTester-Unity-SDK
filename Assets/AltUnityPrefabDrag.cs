using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Altom.AltUnityTester.UI 
{

    public class AltUnityPrefabDrag : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public void OnBeginDrag(PointerEventData eventData)
        {
            var group = eventData.pointerDrag.gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
    #if ENABLE_LEGACY_INPUT_MANAGER
            eventData.pointerDrag.gameObject.transform.position = Input.mousePosition;
    #else
            eventData.pointerDrag.gameObject.transform.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
    #endif
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var canvasGroup = eventData.pointerDrag.gameObject.GetComponent<CanvasGroup>();
            Destroy(canvasGroup);
        }

    }

}