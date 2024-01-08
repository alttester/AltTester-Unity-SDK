using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltImageControllerScene5 : MonoBehaviour
{

    GameObject image;
    // Update is called once per frame
    void Start()
    {
        image = GameObject.Find("Canvas/Image");
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            image.SetActive(!image.activeSelf);
        }
    }
}
