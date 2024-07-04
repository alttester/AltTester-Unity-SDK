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
using UnityEngine;
using System.Collections;
namespace AltTester.AltTesterUnitySDK.InputModule
{

    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        public void StartCoroutineFromExternal(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}