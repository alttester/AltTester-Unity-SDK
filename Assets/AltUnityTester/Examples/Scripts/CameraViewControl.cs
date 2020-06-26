using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewControl : MonoBehaviour
{

    public Camera MainCamera;

    public Camera SecondaryCamera;

    private int cameraMode;
	// Use this for initialization
	void Awake ()
	{
	   cameraMode = 0;
	    SecondaryCamera.enabled = false;
	}

    public void ChangeCameraMode()
    {
        cameraMode++;
        if (cameraMode > 2)
            cameraMode = 0;
        switch (cameraMode)
        {
            case 0:
                MainCamera.rect=new Rect(0,0,1,1);
                SecondaryCamera.enabled = false;
                MainCamera.enabled = true;
                break;
            case 1:
                SecondaryCamera.rect = new Rect(0, 0, 1, 1);
                MainCamera.enabled = false;
                SecondaryCamera.enabled = true;
                break;
            case 2:
                MainCamera.rect=new Rect(0,0,0.5f,1);
                SecondaryCamera.rect=new Rect(0.5f,0,1,1);
                MainCamera.enabled = true;
                SecondaryCamera.enabled = true;
                break;
        }
    }

    public void RotateMainCamera()
    {
        MainCamera.transform.rotation*=Quaternion.Euler(0,180,0);
    }
    public void RotateSecondaryCamera()
    {
        SecondaryCamera.transform.rotation *= Quaternion.Euler(0, 180, 0);
    }
}
