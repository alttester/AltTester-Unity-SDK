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
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldBaseFindObjetsCommand<T> : AltCommand<BaseFindObjectsParams, T>
    {
        protected readonly BaseFindObjectsParams FindObjectsParams;

        protected AltOldBaseFindObjetsCommand(BaseFindObjectsParams cmdParams) : base(cmdParams)
        {
            this.FindObjectsParams = cmdParams;
        }
        public override T Execute()
        {
            throw new System.NotImplementedException();
        }
        public List<UnityEngine.GameObject> FindObjects(UnityEngine.GameObject gameObject, OldBoundCondition boundCondition, bool singleObject, bool enabled)
        {
            if (boundCondition == null)
            {
                if (gameObject == null)
                    return new List<GameObject>();
                else
                    return new List<GameObject>() { gameObject };
            }

            if (boundCondition.Type == OldBoundType.Parent)
            {
                //   /name/../../name
                if (gameObject == null)
                {
                    throw new InvalidPathException("Cannot select a parent of root object");
                }
                var parent = gameObject.transform.parent != null ? gameObject.transform.parent.gameObject : null;
                return FindObjects(parent, boundCondition.NextBound, singleObject, enabled);
            }

            List<UnityEngine.GameObject> objectsToCheck = getGameObjectsToCheck(gameObject);
            List<UnityEngine.GameObject> objectsFound = new List<UnityEngine.GameObject>();
            List<UnityEngine.GameObject> objectsMatched = new List<UnityEngine.GameObject>();
            foreach (var objectToCheck in objectsToCheck)
            {
                GameObject nextObjectToCheck;
                if (checkValidVisibility(objectToCheck, enabled))
                {
                    nextObjectToCheck = objectMatchesConditions(objectToCheck, boundCondition, enabled);
                    if (nextObjectToCheck != null)
                    {
                        objectsMatched.Add(objectToCheck);
                    }
                }

            }
            if (boundCondition.Indexer != null)
            {
                try
                {
                    objectsFound.AddRange(FindObjects(objectsMatched[(boundCondition.Indexer.Index < 0 ? objectsMatched.Count : 0) + boundCondition.Indexer.Index], boundCondition.NextBound, singleObject, enabled));
                }
                catch (System.Exception)
                {

                }
            }
            else
            {
                foreach (var matched in objectsMatched)
                {
                    objectsFound.AddRange(FindObjects(matched, boundCondition.NextBound, singleObject, enabled));
                }
            }
            if (singleObject && objectsFound.Count > 0)
            {
                return objectsFound;
            }
            if (boundCondition.Type != OldBoundType.DirectChildren)
                foreach (var objectToCheck in objectsToCheck)
                {

                    objectsFound.AddRange(FindObjects(objectToCheck, boundCondition, singleObject, enabled));
                    if (singleObject && objectsFound.Count > 0)
                    {
                        return objectsFound;
                    }
                }

            return objectsFound;
        }
        protected UnityEngine.Camera GetCamera(By cameraBy, string cameraValue)
        {

            if (cameraBy == By.NAME)
            {
                var cameraValueSplit = cameraValue.Split('/');
                var cameraName = cameraValueSplit[cameraValueSplit.Length - 1];
                return UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));

            }
            else
            {
                var cameraValueProcessed = new OldPathSelector(cameraValue);
                var gameObjectsCameraFound = FindObjects(null, cameraValueProcessed.FirstBound, false, true);
                return UnityEngine.Camera.allCameras.ToList().Find(c => gameObjectsCameraFound.Find(d => c.gameObject.GetInstanceID() == d.GetInstanceID()));
            }

        }
        private bool checkValidVisibility(GameObject objectToCheck, bool enabled)
        {
            return !enabled || (enabled && objectToCheck.activeInHierarchy);
        }
        private GameObject objectMatchesConditions(GameObject objectToCheck, OldBoundCondition boundCondition, bool enabled)
        {
            var currentCondition = boundCondition.FirstSelector;
            GameObject objectMatched = null;
            while (currentCondition != null)
            {

                objectMatched = currentCondition.MatchCondition(objectToCheck, enabled);
                if (objectMatched == null)
                {
                    return null;
                }
                currentCondition = currentCondition.NextSelector;
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


    }
}