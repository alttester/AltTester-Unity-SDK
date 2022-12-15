using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltButtonHoldTimer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float initialTime = 0;
    public void OnPointerDown(PointerEventData eventData)
    {
        initialTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject.Find("ChineseLetters").GetComponent<Text>().text = (Time.time - initialTime).ToString();
        initialTime = 0;
    }


}
