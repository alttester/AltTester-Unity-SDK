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
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltBaseFindObjectsCommand<T> : AltCommand<BaseGameFindObjectParams, T>
    {
        protected readonly BaseGameFindObjectParams FindObjectsParams;

        protected AltBaseFindObjectsCommand(BaseGameFindObjectParams cmdParams) : base(cmdParams)
        {
            this.FindObjectsParams = cmdParams;
        }
        public override T Execute()
        {
            throw new System.NotImplementedException();
        }
        public List<UnityEngine.GameObject> FindObjects(UnityEngine.GameObject gameObject, List<BoundCondition> boundConditions, int index, bool singleObject, bool enabled)
        {
            //Stop condition
            if (index == boundConditions.Count)
            {
                if (gameObject == null)
                    return new List<GameObject>();
                else
                    return new List<GameObject>() { gameObject };
            }

            if (boundConditions[index].Type == BoundType.Parent)
            {
                //   /name/../../name
                if (gameObject == null)
                {
                    throw new InvalidPathException("Cannot select a parent of root object");
                }
                var parent = gameObject.transform.parent != null ? gameObject.transform.parent.gameObject : null;
                var newIndex = index + 1;
                return FindObjects(parent, boundConditions, newIndex, singleObject, enabled);
            }

            List<UnityEngine.GameObject> objectsToCheck = getGameObjectsToCheck(gameObject);
            List<UnityEngine.GameObject> objectsFound = new List<UnityEngine.GameObject>();
            List<UnityEngine.GameObject> objectsMatched = new List<UnityEngine.GameObject>();
            foreach (var objectToCheck in objectsToCheck)
            {

                GameObject nextObjectToCheck;
                if (checkValidVisibility(objectToCheck, enabled))
                {
                    nextObjectToCheck = objectMatchesConditions(objectToCheck, boundConditions[index], enabled);
                    if (nextObjectToCheck != null)
                    {
                        objectsMatched.Add(objectToCheck);
                    }
                }

            }
            if (boundConditions[index].Indexer != null)
            {
                try
                {
                    int newIndex = index + 1;
                    objectsFound.AddRange(FindObjects(objectsMatched[(boundConditions[index].Indexer.Index < 0 ? objectsMatched.Count : 0) + boundConditions[index].Indexer.Index], boundConditions, newIndex, singleObject, enabled));
                }
                catch (System.Exception)
                {

                }
            }
            else
            {
                foreach (var matched in objectsMatched)
                {
                    int newIndex = index + 1;
                    objectsFound.AddRange(FindObjects(matched, boundConditions, newIndex, singleObject, enabled));
                }
            }
            if (singleObject && objectsFound.Count > 0)
            {
                return objectsFound;
            }
            if (boundConditions[index].Type != BoundType.DirectChildren)
                foreach (var objectToCheck in objectsToCheck)
                {
                    objectsFound.AddRange(FindObjects(objectToCheck, boundConditions, index, singleObject, enabled));
                    if (singleObject && objectsFound.Count > 0)
                    {
                        return objectsFound;
                    }
                }

            return objectsFound;
        }
        protected UnityEngine.Camera GetCamera(List<BoundCondition> cameraConditons)
        {
            var gameObjectsCameraFound = FindObjects(null, cameraConditons, 0, false, true);
            return UnityEngine.Camera.allCameras.ToList().Find(c => gameObjectsCameraFound.Find(d => c.gameObject.GetInstanceID() == d.GetInstanceID()));

        }
        private bool checkValidVisibility(GameObject objectToCheck, bool enabled)
        {
            return !enabled || (enabled && objectToCheck.activeInHierarchy);
        }
        private GameObject objectMatchesConditions(GameObject objectToCheck, BoundCondition boundCondition, bool enabled)
        {
            GameObject objectMatched = null;
            foreach (var selector in boundCondition.Selectors)
            {
                objectMatched = SelectorConditionController.MatchCondition(selector, objectToCheck, enabled);
                if (objectMatched == null)
                {
                    return null;
                }
            }
            return objectMatched;

        }


        private List<UnityEngine.GameObject> getGameObjectsToCheck(UnityEngine.GameObject gameObject)
        {
            return gameObject == null ? getAllRootObjects() : getAllChildren(gameObject);
        }

        private List<UnityEngine.GameObject> getAllChildren(UnityEngine.GameObject gameObject)
        {
            List<UnityEngine.GameObject> objectsToCheck = new List<UnityEngine.GameObject>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                objectsToCheck.Add(gameObject.transform.GetChild(i).gameObject);
            }
            return objectsToCheck;
        }

        private List<UnityEngine.GameObject> getAllRootObjects()
        {
            List<UnityEngine.GameObject> objectsToCheck = new List<UnityEngine.GameObject>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    objectsToCheck.Add(rootGameObject);
                }
            }
            foreach (var destroyOnLoadObject in AltRunner.GetDontDestroyOnLoadObjects())
            {
                objectsToCheck.Add(destroyOnLoadObject);

            }
            return objectsToCheck;
        }

        /// <summary>
        /// Checks if the camera condition is //*
        /// </summary>
        /// <param name="cameraConditions"></param>
        /// <returns>True if camera condition is different that //* otherwise returns false</returns>
        protected bool IsCameraSpecified(List<BoundCondition> cameraConditions)
        {
            return !(cameraConditions.Count == 1 && cameraConditions[0].Selector == "//" && cameraConditions[0].Selectors.Count == 1 && cameraConditions[0].Selectors[0].Selector == "*");
        }


    }
}
