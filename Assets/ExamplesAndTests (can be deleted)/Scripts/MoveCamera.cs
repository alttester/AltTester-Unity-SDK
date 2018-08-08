using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveCamera : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
        float cameraTiltx = CrossPlatformInputManager.GetAxis("Mouse Y");
        float cameraTilty = CrossPlatformInputManager.GetAxis("Mouse X");
        Vector3 rotateValue = new Vector3(cameraTiltx, cameraTilty * -1, 0);
        //Debug.Log(rotateValue);
        this.transform.eulerAngles = transform.eulerAngles - rotateValue;
    }
}
