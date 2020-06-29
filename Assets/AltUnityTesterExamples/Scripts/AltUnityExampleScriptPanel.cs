using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltUnityExampleScriptPanel : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    float speed = 10f;
    // Start is called before the first frame update

    public Color normalColor;
	  public Color highlightColor;

    public Image sourceImage;
    void Start()
    {
        sourceImage = gameObject.GetComponent<Image>();

        normalColor = sourceImage.color;
        highlightColor = normalColor;
    }

    // Update is called once per frame
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
