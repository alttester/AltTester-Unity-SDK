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
using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.InputModule;
using AltTester.AltTesterUnitySDK.Logging;
using AltTester.AltTesterUnitySDK.Notification;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltRunner : UnityEngine.MonoBehaviour
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public static readonly string VERSION = "2.1.2";
        public static AltRunner _altRunner;
        public static AltResponseQueue _responseQueue;
        public AltInstrumentationSettings InstrumentationSettings = null;


        [UnityEngine.Space]
        public bool RunOnlyInDebugMode = true;
        public UnityEngine.Shader outlineShader;
        public UnityEngine.GameObject panelHightlightPrefab;


        #region MonoBehaviour

        protected void Awake()
        {
#if !ALTTESTER
            logger.Error("ALTTESTER needs to be added to 'Scripting Define Symbols'");
            Destroy(this.gameObject);
            return;

#else
            if (_altRunner != null)
            {
                Destroy(this.gameObject);
                return;
            }

            if (RunOnlyInDebugMode && !UnityEngine.Debug.isDebugBuild)
            {
                logger.Error("AltTester(R) runs only on Debug build");
                Destroy(this.gameObject);
                return;
            }

            ServerLogManager.SetupAltServerLogging(new Dictionary<AltLogger, AltLogLevel> { { AltLogger.File, AltLogLevel.Debug }, { AltLogger.Unity, AltLogLevel.Debug } });

            _altRunner = this;
            DontDestroyOnLoad(this);
#endif
        }
        protected void Start()
        {
            _responseQueue = new AltResponseQueue();

        }

        protected void Update()
        {
            _responseQueue.Cycle();
        }

        #endregion
        #region public methods

        public void OnApplicationPause(bool pauseStatus)
        {
            AltTesterApplicationPausedNotification.OnPause(pauseStatus);
        }

        public AltObject GameObjectToAltObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
        {
            UnityEngine.Vector3 position;

            int cameraId;
            //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
            // if there is no camera in the scene it will return as screen position x:-1 y=-1, z=-1 and cameraId=-1
            try
            {
                if (camera == null)
                {
                    cameraId = FindObjectViaRayCast.FindCameraThatSeesObject(altGameObject, out position);
                }
                else
                {
                    position = FindObjectViaRayCast.GetObjectScreenPosition(altGameObject, camera);
                    cameraId = camera.GetInstanceID();
                }
            }
            catch (Exception)
            {
                position = UnityEngine.Vector3.one * -1;
                cameraId = -1;
            }

            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();

            var altObject = new AltObject(
                name: altGameObject.name,
                id: altGameObject.GetInstanceID(),
                x: Convert.ToInt32(UnityEngine.Mathf.Round(position.x)),
                y: Convert.ToInt32(UnityEngine.Mathf.Round(position.y)),
                z: Convert.ToInt32(UnityEngine.Mathf.Round(position.z)),//if z is negative object is behind the camera
                mobileY: Convert.ToInt32(UnityEngine.Mathf.Round(UnityEngine.Screen.height - position.y)),
                type: "",
                enabled: altGameObject.activeSelf,
                worldX: altGameObject.transform.position.x,
                worldY: altGameObject.transform.position.y,
                worldZ: altGameObject.transform.position.z,
                idCamera: cameraId,
                transformId: altGameObject.transform.GetInstanceID(),
                transformParentId: transformParentId);
            return altObject;
        }

        public AltObjectLight GameObjectToAltObjectLight(UnityEngine.GameObject altGameObject)
        {
            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();
            AltObjectLight altObject = new AltObjectLight(
                name: altGameObject.name,
                id: altGameObject.GetInstanceID(),
                enabled: altGameObject.activeSelf,
                idCamera: 0,
                transformId: altGameObject.transform.GetInstanceID(),
                transformParentId: transformParentId);

            return altObject;
        }

        public static UnityEngine.GameObject[] GetDontDestroyOnLoadObjects()
        {
            UnityEngine.GameObject temp = null;
            try
            {
                temp = new UnityEngine.GameObject();
                DontDestroyOnLoad(temp);
                UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
                DestroyImmediate(temp);
                temp = null;

                return dontDestroyOnLoad.GetRootGameObjects();
            }
            finally
            {
                if (temp != null)
                    DestroyImmediate(temp);
            }
        }

        public static UnityEngine.GameObject GetGameObject(int objectId)
        {
            foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
            {
                if (gameObject.GetInstanceID() == objectId)
                    return gameObject;
            }
            throw new NotFoundException("Object not found");
        }

        public UnityEngine.Camera FoundCameraById(int id)
        {
            foreach (var camera in UnityEngine.Camera.allCameras)
            {
                if (camera.GetInstanceID() == id)
                    return camera;
            }

            return null;
        }

        public System.Collections.IEnumerator RunActionAfterEndOfFrame(Action action)
        {
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new UnityEngine.WaitForEndOfFrame();
            action();
        }


        #endregion
        #region private methods


        #endregion
    }
}
