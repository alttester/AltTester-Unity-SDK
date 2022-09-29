using System.Collections;
using UnityEngine;

public class AltExampleScriptActiveStateToggler : MonoBehaviour
{

    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
