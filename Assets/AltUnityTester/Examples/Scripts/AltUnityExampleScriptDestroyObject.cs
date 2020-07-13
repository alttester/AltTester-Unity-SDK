using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltUnityExampleScriptDestroyObject : MonoBehaviour {

    public void Awake()
    {
        Destroy(gameObject, 5);
    }
}
