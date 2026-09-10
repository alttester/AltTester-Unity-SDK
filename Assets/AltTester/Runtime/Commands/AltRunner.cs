/*
    Copyright(C) 2026 Altom Consulting
    
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using AltTester;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.InputModule;
using AltTester.AltTesterUnitySDK.Logging;
using AltTester.AltTesterUnitySDK.Notification;
using UnityEngine;
using UnityEngine.UIElements;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltRunner : UnityEngine.MonoBehaviour
    {
        private static readonly NLog.Logger logger = AltTesterLogManager.Instance.GetCurrentClassLogger();

        public static string VERSION => AltTesterVersion.VERSION;
        public static AltRunner _altRunner;
        public static AltResponseQueue _responseQueue;
        public AltInstrumentationSettings InstrumentationSettings = null;
        public Canvas panelHightlightCanvas;


        [UnityEngine.Space]
        public bool RunOnlyInDebugMode = true;
        public UnityEngine.Shader outlineShader;
        public UnityEngine.GameObject panelHightlightPrefab;


        #region MonoBehaviour

        protected void Awake()
        {
#if !ALTTESTER
            logger.Error("ALTTESTER needs to be added to 'Scripting Define Symbols'");
            // Deactivate immediately (Destroy only takes effect at end of frame): this skips
            // Start() on every component under this object (AltDialog's reconnect/version-check
            // coroutines, InvokeRepeating, etc.) instead of letting them run once before cleanup.
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;

#else
            if (_altRunner != null)
            {
                // Duplicate instance (e.g. the prefab is present in more than one scene). Deactivate
                // immediately so no component under it runs its (potentially expensive) Start() logic
                // before the deferred Destroy below actually removes it.
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
                return;
            }

            if (RunOnlyInDebugMode && !UnityEngine.Debug.isDebugBuild)
            {
                logger.Error("AltTester(R) runs only on Debug build");
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
                return;
            }

            AltTesterLogManager.SetupAltServerLogging(new Dictionary<AltLogger, AltLogLevel> { { AltLogger.File, AltLogLevel.Debug }, { AltLogger.Unity, AltLogLevel.Debug } });

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
                    cameraId = camera.GetObjectInstanceId();
                }
            }
            catch (Exception)
            {
                position = UnityEngine.Vector3.one * -1;
                cameraId = -1;
            }

            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetObjectInstanceId();

            var altObject = new AltObject(
                name: altGameObject.name,
                id: altGameObject.GetObjectInstanceId(),
                x: (position.x < int.MinValue) ? int.MinValue :
                   (position.x > int.MaxValue) ? int.MaxValue :
                   Convert.ToInt32(Mathf.Round(position.x)),

                y: (position.y < int.MinValue) ? int.MinValue :
                   (position.y > int.MaxValue) ? int.MaxValue :
                    Convert.ToInt32(Mathf.Round(position.y)),

                z: (position.z < int.MinValue) ? int.MinValue :
                   (position.z > int.MaxValue) ? int.MaxValue :
                   Convert.ToInt32(Mathf.Round(position.z)),//if z is negative object is behind the camera
                mobileY: (position.y < int.MinValue + 1) ? int.MaxValue - 1 :
                         (position.y > int.MaxValue - 1) ? int.MinValue + 1 :
                          Convert.ToInt32(Mathf.Round(UnityEngine.Screen.height - position.y)),
                type: "",
                enabled: altGameObject.activeSelf,
                worldX: altGameObject.transform.position.x,
                worldY: altGameObject.transform.position.y,
                worldZ: altGameObject.transform.position.z,
                idCamera: cameraId,
                transformId: altGameObject.transform.GetObjectInstanceId(),
                transformParentId: transformParentId);
            return altObject;
        }


        public AltObjectLight GameObjectToAltObjectLight(UnityEngine.GameObject altGameObject)
        {
            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetObjectInstanceId();
            AltObjectLight altObject = new AltObjectLight(
                name: altGameObject.name,
                type: "GameObject",
                id: altGameObject.GetObjectInstanceId(),
                enabled: altGameObject.activeSelf,
                idCamera: 0,
                transformId: altGameObject.transform.GetObjectInstanceId(),
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

        public static UnityEngine.GameObject GetGameObject(int altObjectID, bool throwError = true)
        {

            foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
            {
                if (gameObject.GetObjectInstanceId() == altObjectID)
                    return gameObject;
            }
            if (throwError)
                throw new NotFoundException("Object not found");
            return null;
        }

        public UnityEngine.Camera FoundCameraById(int id)
        {
            foreach (var camera in UnityEngine.Camera.allCameras)
            {
                if (camera.GetObjectInstanceId() == id)
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
