/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltExampleScriptDestroyObject : MonoBehaviour
{

    public void Awake()
    {
        Destroy(gameObject, 5);
    }
}
