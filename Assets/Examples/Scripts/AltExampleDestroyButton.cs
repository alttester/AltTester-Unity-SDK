using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltExampleDestroyButton : MonoBehaviour
{
    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }
}
