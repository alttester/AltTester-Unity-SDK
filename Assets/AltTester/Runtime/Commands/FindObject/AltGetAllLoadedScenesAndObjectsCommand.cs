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

using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetAllLoadedScenesAndObjectsCommand : AltBaseFindObjectsCommand<List<AltObjectLight>>
    {
        public AltGetAllLoadedScenesAndObjectsCommand(BaseGameFindObjectParams cmdParams) : base(cmdParams) { }

        public override List<AltObjectLight> Execute()
        {
            List<AltObjectLight> foundObjects = new List<AltObjectLight>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                foundObjects.Add(new AltObjectLight(scene.name));
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    if (CommandParams.enabled == false || rootGameObject.activeSelf)
                    {
                        foundObjects.Add(AltRunner._altRunner.GameObjectToAltObjectLight(rootGameObject));
                        foundObjects.AddRange(getAllChildren(rootGameObject));
                    }
                }
            }

            var doNotDestroyOnLoadObjects = AltRunner.GetDontDestroyOnLoadObjects();
            if (doNotDestroyOnLoadObjects.Length != 0)
            {
                foundObjects.Add(new AltObjectLight("DontDestroyOnLoad"));
            }
            foreach (var destroyOnLoadObject in AltRunner.GetDontDestroyOnLoadObjects())
            {
                if (CommandParams.enabled == false || destroyOnLoadObject.activeSelf)
                {
                    foundObjects.Add(AltRunner._altRunner.GameObjectToAltObjectLight(destroyOnLoadObject));
                    foundObjects.AddRange(getAllChildren(destroyOnLoadObject));
                }
            }
            return foundObjects;

        }
        private List<AltObjectLight> getAllChildren(UnityEngine.GameObject gameObject)
        {
            List<AltObjectLight> children = new List<AltObjectLight>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                if (CommandParams.enabled == false || child.activeSelf)
                {
                    children.Add(AltRunner._altRunner.GameObjectToAltObjectLight(child));
                    children.AddRange(getAllChildren(child));
                }

            }
            return children;
        }
    }
}
