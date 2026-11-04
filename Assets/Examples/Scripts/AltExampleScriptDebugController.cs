/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class AltExampleScriptDebugController : MonoBehaviour
{
    public Text lastKeyDown;
    public Text lastKeyUp;
    public Text lastKeyPressed;
    public Text lastButtonDown;
    public Text lastButtonUp;
    public Text lastButtonPressed;
    public Text mousePosition;
    public Text scroll;
    public Text lastAxis;
    public Text lastAxisValue;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    lastKeyDown.text = ((int)kcode).ToString();
            }
        }
        if (Input.anyKey)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                    lastKeyPressed.text = ((int)kcode).ToString();
            }
        }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(kcode))
                lastKeyUp.text = ((int)kcode).ToString();
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            lastAxis.text = "Horizontal";
            lastAxisValue.text = Input.GetAxis("Horizontal").ToString();

        }
        if (Input.GetAxis("Vertical") != 0)
        {
            lastAxis.text = "Vertical";
            lastAxisValue.text = Input.GetAxis("Vertical").ToString();
        }

        mousePosition.text = Input.mousePosition.ToString();
        scroll.text = Input.mouseScrollDelta.ToString();
    }
}
