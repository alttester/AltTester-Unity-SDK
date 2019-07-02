using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {
    public int speed = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

        Vector3 movement = new Vector3();
        movement.x += Input.GetAxis("Horizontal")*Time.deltaTime*speed;
        movement.z += Input.GetAxis("Vertical") * Time.deltaTime*speed;
        transform.position += movement;

    }
}
