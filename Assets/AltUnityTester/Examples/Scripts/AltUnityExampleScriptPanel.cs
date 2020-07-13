using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltUnityExampleScriptPanel : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
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
        highlightColor=Color.yellow;
        sourceImage.color = highlightColor;
    }

	public void OnPointerUp(PointerEventData data)
	{
		
	  highlightColor = normalColor;
		sourceImage.color = normalColor;
	}

}
