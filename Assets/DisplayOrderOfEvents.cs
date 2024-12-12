using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayOrderOfEvents : MonoBehaviour
{
    public int count = 0;
    public TMP_Text OnScroll;
    public TMP_Text OnBeginDrag;
    public TMP_Text OnEndDrag;
    public TMP_Text OnDrag;
    public TMP_Text OnInitializePotentialDrag;
    public TMP_Text OnDeselect;
    public TMP_Text OnSelect;
    public TMP_Text OnMove;
    public TMP_Text OnPointerEnter;
    public TMP_Text OnPointerExit;
    public TMP_Text OnPointerDown;
    public TMP_Text OnPointerUp;
    public TMP_Text OnPointerClick;
    public TMP_Text OnSubmit;

    public void Start()
    {
        OnScroll = GameObject.Find("OnScroll").GetComponent<TMP_Text>();
        OnBeginDrag = GameObject.Find("OnBeginDrag").GetComponent<TMP_Text>();
        OnEndDrag = GameObject.Find("OnEndDrag").GetComponent<TMP_Text>();
        OnDrag = GameObject.Find("OnDrag").GetComponent<TMP_Text>();
        OnInitializePotentialDrag = GameObject.Find("OnInitializePotentialDrag").GetComponent<TMP_Text>();
        OnDeselect = GameObject.Find("OnDeselect").GetComponent<TMP_Text>();
        OnSelect = GameObject.Find("OnSelect").GetComponent<TMP_Text>();
        OnMove = GameObject.Find("OnMove").GetComponent<TMP_Text>();
        OnPointerEnter = GameObject.Find("OnPointerEnter").GetComponent<TMP_Text>();
        OnPointerExit = GameObject.Find("OnPointerExit").GetComponent<TMP_Text>();
        OnPointerDown = GameObject.Find("OnPointerDown").GetComponent<TMP_Text>();
        OnPointerUp = GameObject.Find("OnPointerUp").GetComponent<TMP_Text>();
        OnPointerClick = GameObject.Find("OnPointerClick").GetComponent<TMP_Text>();
        OnSubmit = GameObject.Find("OnSubmit").GetComponent<TMP_Text>();

        count = 0;
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnScrollEventDetected()
    {
        OnScroll.text += " " + count++.ToString();
    }

    public void OnBeginDragEventDetected()
    {
        OnBeginDrag.text += " " + count++.ToString();
    }

    public void OnEndDragEventDetected()
    {
        OnEndDrag.text += " " + count++.ToString();
    }

    public void OnDragEventDetected()
    {
        OnDrag.text += " " + count++.ToString();
    }

    public void OnInitializePotentialDragEventDetected()
    {
        OnInitializePotentialDrag.text += " " + count++.ToString();
    }

    public void OnDeselectEventDetected()
    {
        OnDeselect.text += " " + count++.ToString();
    }

    public void OnSelectEventDetected()
    {
        OnSelect.text += " " + count++.ToString();
    }

    public void OnMoveEventDetected()
    {
        OnMove.text += " " + count++.ToString();
    }

    public void OnPointerEnterEventDetected()
    {
        OnPointerEnter.text += " " + count++.ToString();
    }

    public void OnPointerExitEventDetected()
    {
        OnPointerExit.text += " " + count++.ToString();
    }

    public void OnPointerDownEventDetected()
    {
        OnPointerDown.text += " " + count++.ToString();
    }

    public void OnPointerUpEventDetected()
    {
        OnPointerUp.text += " " + count++.ToString();
    }

    public void OnPointerClickEventDetected()
    {
        OnPointerClick.text += " " + count++.ToString();
    }

    public void OnSubmitEventDetected()
    {
        OnSubmit.text += " " + count++.ToString();
    }
}
