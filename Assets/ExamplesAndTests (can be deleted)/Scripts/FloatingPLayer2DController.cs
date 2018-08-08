using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class FloatingPLayer2DController : MonoBehaviour {
    public float moveForce = 5, boostMultiplier = 1000;
    Rigidbody2D myBody;
    public Material material;
    public Text forDebug;
	void Start () {
        myBody = this.GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vector2 = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal") * moveForce, CrossPlatformInputManager.GetAxis("Vertical") * moveForce);
        //bool pressedButton = CrossPlatformInputManager.GetButton("Jump");
        //if (pressedButton)
        //{
        //    this.gameObject.transform.position =  new Vector3(10, 10, 10);
        //}
        //else
        //{
        //    this.gameObject.transform.position = new Vector3(0, 0, 0);
        //}
        myBody.AddForce(vector2);

        if (vector2 == Vector2.zero)
        {
            myBody.velocity = Vector2.zero;
        }
	}
}
