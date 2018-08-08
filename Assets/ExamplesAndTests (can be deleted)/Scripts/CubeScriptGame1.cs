using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CubeScriptGame1 : MonoBehaviour {

    public float moveForce =5;
    Rigidbody2D myBody;

    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vector2 = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal") * moveForce, CrossPlatformInputManager.GetAxis("Vertical") * moveForce);
        if (vector2 != Vector2.zero)
        {
            Debug.Log(vector2);
        }
        bool pressedButton = CrossPlatformInputManager.GetButton("Jump");
        if (pressedButton)
        {
            this.gameObject.transform.localScale = new Vector3(10, 10, 10);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        myBody.AddForce(vector2*5);

        if (vector2 == Vector2.zero)
        {
            myBody.velocity = Vector2.zero;
        }
    }
}
