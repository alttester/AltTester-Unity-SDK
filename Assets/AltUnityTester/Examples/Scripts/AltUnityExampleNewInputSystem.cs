using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class AltUnityExampleNewInputSystem : MonoBehaviour
{
    public bool wasClicked = false;

    void Update()
    {
        wasClicked = Mouse.current.position.ReadValue() != Vector2.zero;
    }
  
}
