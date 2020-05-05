using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public Text counterText;
    int counter = 0;
    void Start()
    {
        counter = 0;
        counterText.text = counter.ToString();
    }

    // Update is called once per frame
    public void ButtonPressed()
    {
        counter++;
        counterText.text = counter.ToString();
    }
}
