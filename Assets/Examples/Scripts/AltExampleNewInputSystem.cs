/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;




public class AltExampleNewInputSystem : MonoBehaviour
{
    int jumpCounter = 0;
    public Vector3 previousAcceleration = Vector3.zero;
    public Text counterText;
    public Text actionText;
    public Rigidbody capsuleRigidBody;
    public static Mouse Mouse;
    public static Touchscreen Touchscreen;

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
        capsuleRigidBody.GetComponent<Rigidbody>().AddForce(Vector3.up * 5.5f, ForceMode.Impulse);
    }

}
