using System;
using System.Collections;
using UnityEngine;

public class AltDragController : MonoBehaviour
{
    private Vector3 offset;
    private bool mouseButtonClick;
    private Vector3 defaultPos;

    public void Start()
    {
        defaultPos = transform.localPosition;
    }

    public void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        offset = transform.position - mousePos;
        mouseButtonClick = true;
    }

    public void OnMouseDrag()
    {
        if (mouseButtonClick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    IEnumerator ResetMoveAnimation(Vector3 startPos, Vector3 endPos)
    {
        float i = 0;
        Vector3 newPos = Vector3.zero;
        while (i < 1)
        {
            i += Time.deltaTime * 5;
            newPos = Vector3.Lerp(startPos, endPos, i);
            this.transform.localPosition = newPos;
            yield return null;
        }
        GetComponent<BoxCollider2D>().enabled = true;

    }

    public void OnMouseUp()
    {
        OnBlockSet();
    }

    private void OnBlockSet()
    {
        if (mouseButtonClick)
        {
            Transform nearestT = AltGameManager.Ins.GetNearestBlock(transform.position);
            if (Vector3.Distance(nearestT.position, transform.position) < 1f)
            {
                mouseButtonClick = false;
                nearestT.GetComponent<AltHolder>().isFull = true;
                transform.position = nearestT.position;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                StartCoroutine(ResetMoveAnimation(transform.localPosition, defaultPos));
            }
            mouseButtonClick = false;
        }
    }

    public void ResetObject()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(ResetMoveAnimation(transform.localPosition, defaultPos));
    }
}
