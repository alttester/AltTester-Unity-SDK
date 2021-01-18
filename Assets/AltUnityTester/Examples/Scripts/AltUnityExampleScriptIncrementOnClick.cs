using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltUnityExampleScriptIncrementOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public Text counterText;
    int counter = 0;

    int keyPressDownCounter = 0;
    int keyPressUpCounter = 0;
    void Start()
    {
        counter = 0;
        counterText.text = counter.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            keyPressDownCounter++;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            keyPressUpCounter++;
        }
        
    }
    public void ButtonPressed()
    {
        counter++;
        counterText.text = counter.ToString();
    }
}
