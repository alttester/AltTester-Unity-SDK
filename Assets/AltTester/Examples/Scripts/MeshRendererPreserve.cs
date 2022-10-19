using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererPreserve : MonoBehaviour
{
    string name;
    // Start is called before the first frame update
    void Start()
    {

        var MeshRenderer = gameObject.GetComponent<Renderer>();
        foreach (var material in MeshRenderer.materials)
        {
            name = material.shader.name;
        }
    }
}
