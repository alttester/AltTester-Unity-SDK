/*
    Copyright(C) 2026 Altom Consulting
    
*/

using System;
using System.Collections.Generic;
using System.Reflection;
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
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public static readonly string VERSION = "2.3.1";
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
                transformId: altGameObject.transform.GetInstanceID(),
                transformParentId: transformParentId);
            return altObject;
        }
        public static Vector2 GetScreenPosition(VisualElement visualElement)
        {
            var screenCenterPos = visualElement.worldBound.center;

            UIDocument[] uIDocuments = GameObject.FindObjectsOfType<UIDocument>();

            Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

            float scaleFactor = ScaleFactor(uIDocuments[0].panelSettings, currentResolution);
            if (uIDocuments[0].panelSettings.scaleMode == PanelScaleMode.ConstantPixelSize)
            {
                scaleFactor = 1f / scaleFactor;
            }
            screenCenterPos *= scaleFactor;
            screenCenterPos.y = Screen.height - screenCenterPos.y;
            return screenCenterPos;
        }

        public static float ScaleFactor(PanelSettings panelSettings, Vector2 screenSize)
        {
            var screenDpi = Screen.dpi;
            float num = 1f;
            switch (panelSettings.scaleMode)
            {
                case PanelScaleMode.ConstantPhysicalSize:
                    {
                        float num3 = (screenDpi == 0f) ? 96f : screenDpi;
                        if (num3 != 0f)
                        {
                            num = panelSettings.referenceDpi / num3;
                        }

                        break;
                    }
                case PanelScaleMode.ScaleWithScreenSize:
                    if (panelSettings.referenceResolution.x * panelSettings.referenceResolution.y != 0)
                    {
                        Vector2 vector = panelSettings.referenceResolution;
                        Vector2 vector2 = new Vector2(screenSize.x / vector.x, screenSize.y / vector.y);
                        float num2 = 0f;
                        switch (panelSettings.screenMatchMode)
                        {
                            case PanelScreenMatchMode.Expand:
                                num2 = Mathf.Min(vector2.x, vector2.y);
                                break;
                            case PanelScreenMatchMode.Shrink:
                                num2 = Mathf.Max(vector2.x, vector2.y);
                                break;
                            default:
                                {
                                    float t = Mathf.Clamp01(panelSettings.match);
                                    num2 = Mathf.Lerp(vector2.x, vector2.y, t);
                                    break;
                                }
                        }

                        if (num2 != 0f)
                        {
                            num = num2;
                        }
                    }

                    break;
            }

            if (panelSettings.scale > 0f)
            {
                return num / panelSettings.scale;
            }

            return 0f;
        }

        public AltObjectLight GameObjectToAltObjectLight(UnityEngine.GameObject altGameObject)
        {
            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();
            AltObjectLight altObject = new AltObjectLight(
                name: altGameObject.name,
                type: "GameObject",
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

        public static UnityEngine.GameObject GetGameObject(int altObjectID, bool throwError = true)
        {

            foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
            {
                if (gameObject.GetInstanceID() == altObjectID)
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
