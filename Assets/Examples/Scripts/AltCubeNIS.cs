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

using UnityEngine;
using UnityEngine.InputSystem;

public class AltCubeNIS : MonoBehaviour
{

    public Vector3 previousAcceleration = Vector3.zero;
    public bool isMoved = false;

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
}
