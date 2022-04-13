using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;




public class AltUnityExampleNewInputSystem : MonoBehaviour
{
    int jumpCounter = 0;
    public bool wasClicked = false;
    public Vector3 previousAcceleration = Vector3.zero;
    public Text counterText;
    public Text actionText;
    public Rigidbody capsuleRigidBody;
    public static Mouse Mouse;
    public static Touchscreen Touchscreen;

    void OnEnable()
    {
#if UNITY_ANDROID
        InputSystem.EnableDevice(Accelerometer.current);
#endif

    }
    protected void OnDisable()
    {
#if UNITY_ANDROID

        InputSystem.DisableDevice(Accelerometer.current);
#endif

    }
    void Update()
    {
        wasClicked = Mouse.current.position.ReadValue() != Vector2.zero;
#if UNITY_ANDROID

        var acceleration = Accelerometer.current.acceleration.ReadValue();
        if (acceleration != previousAcceleration)
        {
            previousAcceleration = acceleration;
            transform.Rotate(acceleration);
        }
#endif

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump" && context.phase is InputActionPhase.Performed)
        {
            if (context.interaction is TapInteraction)
            {
                JumpCapsule();
                actionText.text = "Capsule was tapped!";
            }
            else
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
                {
                    Debug.LogFormat("You hit [{0}]", hit.collider.gameObject.name);
                    if (hit.collider.gameObject.name == "Capsule")
                    {
                        JumpCapsule();
                        actionText.text = "Capsule was clicked!";
                    }
                }

            }
        }
    }
    public void JumpCapsule()
    {
        jumpCounter++;
        counterText.text = jumpCounter.ToString();
        capsuleRigidBody.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

}
