/*
    Copyright(C) 2026 Altom Consulting
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class AltCubeNIS : MonoBehaviour
{
    public Vector3 previousAcceleration = Vector3.zero;
    public bool isMoved = false;

#if UNITY_ANDROID || UNITY_IOS
    protected void OnEnable()
    {
        InputSystem.EnableDevice(Accelerometer.current);

    }
    protected void OnDisable()
    {

        InputSystem.DisableDevice(Accelerometer.current);

    }
    protected void Update()
    {

        var acceleration = Accelerometer.current.acceleration.ReadValue();
        if (acceleration != previousAcceleration)
        {
            previousAcceleration = acceleration;
            transform.position += acceleration * 0.02f;
            isMoved = true;
        }

    }
#endif
}
