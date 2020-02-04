using UnityEngine;
using UnityEngine.EventSystems;

public class AltPopIconDrag : EventTrigger
{

    private bool dragging;
    private float time;
    private bool pressed;
    public void Update()
    {
        if (!dragging && pressed)
        {
            time += Time.deltaTime;
            if (time > 0.5f)
            {
                dragging = true;
            }
        }
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        time = 0;
        pressed = false;
        dragging = false;
    }
}
