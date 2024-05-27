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

using System;
using UnityEngine;
using UnityEngine.UI;

public class AltExampleScriptDebugController : MonoBehaviour
{
    public Text lastKeyDown;
    public Text lastKeyUp;
    public Text lastKeyPressed;
    public Text lastButtonDown;
    public Text lastButtonUp;
    public Text lastButtonPressed;
    public Text mousePosition;
    public Text scroll;
    public Text lastAxis;
    public Text lastAxisValue;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    lastKeyDown.text = ((int)kcode).ToString();
            }
        }
        if (Input.anyKey)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                    lastKeyPressed.text = ((int)kcode).ToString();
            }
        }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(kcode))
                lastKeyUp.text = ((int)kcode).ToString();
        }
#if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER
        Input.GetAxis("Horizontal");
        Input.GetAxis("Vertical");
        lastButtonDown.text = Input.LastButtonDown;
        lastButtonPressed.text = Input.LastButtonPressed;
        lastButtonUp.text = Input.LastButtonUp;
        lastAxis.text = Input.LastAxisName;
        lastAxisValue.text = Input.LastAxisValue.ToString();
#endif

        mousePosition.text = Input.mousePosition.ToString();
        scroll.text = Input.mouseScrollDelta.ToString();
    }
}
