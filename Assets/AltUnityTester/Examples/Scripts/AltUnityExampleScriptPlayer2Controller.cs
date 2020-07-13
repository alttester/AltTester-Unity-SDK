using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltUnityExampleScriptPlayer2Controller : MonoBehaviour {
    public int speed=5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var movement = new Vector3();
        movement.y += Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.K))
            movement.x = -1 * speed * Time.deltaTime+Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.Semicolon))
            movement.x = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.O))
            movement.z = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.L))
            movement.z = -1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
            
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            movement.x = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.x;
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            movement.x = -1 * speed * Time.deltaTime + Input.mouseScrollDelta.x;
        transform.position += movement;

        if (Input.GetButton("Horizontal"))
        {
            Debug.Log("HorizontalPressed");
        }
        if (Input.GetButtonDown("Vertical"))
        {
            Debug.Log("VerticalPressed");

        }

    }
}
