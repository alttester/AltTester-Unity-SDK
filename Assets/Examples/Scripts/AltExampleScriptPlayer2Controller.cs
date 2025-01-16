/*
    Copyright(C) 2025 Altom Consulting

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

public class AltExampleScriptPlayer2Controller : MonoBehaviour
{
    public int speed = 5;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var movement = new Vector3();
        movement.y += Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.K))
            movement.x = -1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.Semicolon))
            movement.x = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.O))
            movement.z = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;
        if (Input.GetKey(KeyCode.L))
            movement.z = -1 * speed * Time.deltaTime + Input.mouseScrollDelta.y;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            movement.x = 1 * speed * Time.deltaTime + Input.mouseScrollDelta.x;
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
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
