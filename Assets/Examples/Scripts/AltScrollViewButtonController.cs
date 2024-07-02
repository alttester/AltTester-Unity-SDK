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
using UnityEngine.UI;

public class AltScrollViewButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public static int Counter = 0;
    void Start()
    {
        Counter = 0;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    public void OnClick()
    {
        Counter++;
        Debug.Log("Tapped:  " + name);
    }
}