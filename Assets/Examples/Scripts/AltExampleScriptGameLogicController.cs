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

public class AltExampleScriptGameLogicController : MonoBehaviour
{
    public GameObject starPrefab;
    public Camera cameraControlled;
    public AltExampleScriptStarCounter starCounter;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cameraControlled.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Floor")
                {
                    starCounter.UpdateStarCounter(true);
                    var position = hit.point;
                    position.y += 1;
                    var star = Instantiate(starPrefab, position, Quaternion.identity);
                    star.GetComponent<AltExampleScriptStarController>().starCounter = starCounter;

                }
            }
        }
    }
}
