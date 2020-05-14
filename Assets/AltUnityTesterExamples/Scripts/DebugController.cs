using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
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
                    lastKeyDown.text=kcode.ToString();
            }
        }
        if (Input.anyKey)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                    lastKeyPressed.text = kcode.ToString();
            }
        }
        
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(kcode))
                lastKeyUp.text = kcode.ToString();
        }
#if ALTUNITYTESTER
        lastButtonDown.text = Input.LastButtonDown;
        lastButtonPressed.text = Input.LastButtonPressed;
        lastButtonUp.text = Input.LastButtonUp;
        lastAxis.text = Input.LastAxisName;
        lastAxisValue.text = Input.LastAxisValue.ToString();
#endif

        mousePosition.text = Input.mousePosition.ToString();
        scroll.text = Input.mouseScrollDelta.ToString();
    }
}