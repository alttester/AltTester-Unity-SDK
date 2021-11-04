using System;
using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Logging;
using Altom.AltUnityTester;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityTester.Logging;
using Altom.AltUnityTester.UI;
using NLog;
using UnityEngine.SceneManagement;

namespace Altom.AltUnityTester
{
    public class AltUnityRunner : UnityEngine.MonoBehaviour
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public static readonly string VERSION = "1.7.0-alpha";
        public static AltUnityRunner _altUnityRunner;
        public static AltResponseQueue _responseQueue;
        public AltUnityInstrumentationSettings InstrumentationSettings = null;


        [UnityEngine.Space]
        public bool RunOnlyInDebugMode = true;
        public UnityEngine.Shader outlineShader;
        public UnityEngine.GameObject panelHightlightPrefab;


        [UnityEngine.Space]
        [UnityEngine.SerializeField]
        public AltUnityInputsVisualiser InputsVisualiser = null;



        #region MonoBehaviour

        protected void Awake()
        {
            if (_altUnityRunner != null)
            {
                Destroy(this.gameObject);
                return;
            }

            if (RunOnlyInDebugMode && !UnityEngine.Debug.isDebugBuild)
            {
                logger.Warn("AltUnityTester runs only on Debug build");
                Destroy(this.gameObject);
                return;
            }

            ServerLogManager.SetupAltUnityServerLogging(new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Unity, AltUnityLogLevel.Debug } });

            _altUnityRunner = this;
            DontDestroyOnLoad(this);
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

        public AltUnityObject GameObjectToAltUnityObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
        {
            UnityEngine.Vector3 position;

            int cameraId;
            //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
            //if there is no camera in the scene it will return as scren position x:-1 y=-1, z=-1 and cameraId=-1
            try
            {
                if (camera == null)
                {
                    cameraId = findCameraThatSeesObject(altGameObject, out position);
                }
                else
                {
                    position = getObjectScreenPosition(altGameObject, camera);
                    cameraId = camera.GetInstanceID();
                }
            }
            catch (Exception)
            {
                position = UnityEngine.Vector3.one * -1;
                cameraId = -1;
            }

            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();

            var altObject = new AltUnityObject(
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

        public AltUnityObjectLight GameObjectToAltUnityObjectLight(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
        {
            int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();
            AltUnityObjectLight altObject = new AltUnityObjectLight(
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

        public static UnityEngine.GameObject GetGameObject(AltUnityObject altUnityObject)
        {
            foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
            {
                if (gameObject.GetInstanceID() == altUnityObject.id)
                    return gameObject;
            }
            throw new NotFoundException("Object not found");
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
            yield return new UnityEngine.WaitForEndOfFrame();
            action();
        }


        public void ShowClick(UnityEngine.Vector2 position)
        {
            if (!InstrumentationSettings.InputVisualizer || InputsVisualiser == null)
                return;

            InputsVisualiser.ShowClick(position);
        }

        public int ShowInput(UnityEngine.Vector2 position, int markId = -1)
        {
            if (!InstrumentationSettings.InputVisualizer || InputsVisualiser == null)
                return -1;

            return InputsVisualiser.ShowContinuousInput(position, markId);
        }

        public static void CopyTo(System.IO.Stream src, System.IO.Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] CompressScreenshot(byte[] screenshotSerialized)
        {
            using (var memoryStreamInput = new System.IO.MemoryStream(screenshotSerialized))
            using (var memoryStreamOutout = new System.IO.MemoryStream())
            {
                using (var gZipStream = new Unity.IO.Compression.GZipStream(memoryStreamOutout, Unity.IO.Compression.CompressionMode.Compress))
                {
                    CopyTo(memoryStreamInput, gZipStream);
                }

                return memoryStreamOutout.ToArray();
            }
        }
        #endregion
        #region private methods
        private UnityEngine.Vector3 getObjectScreenPosition(UnityEngine.GameObject gameObject, UnityEngine.Camera camera)
        {
            var selectedCamera = camera;
            var position = gameObject.transform.position;
            UnityEngine.Canvas canvas = gameObject.GetComponentInParent<UnityEngine.Canvas>();
            if (canvas != null)
            {
                if (canvas.renderMode != UnityEngine.RenderMode.ScreenSpaceOverlay)
                {
                    if (gameObject.GetComponent<UnityEngine.RectTransform>() != null)
                    {
                        UnityEngine.Vector3[] vector3S = new UnityEngine.Vector3[4];
                        gameObject.GetComponent<UnityEngine.RectTransform>().GetWorldCorners(vector3S);
                        position = new UnityEngine.Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);
                    }
                    if (canvas.worldCamera != null)
                    {
                        selectedCamera = canvas.worldCamera;
                    }
                    return selectedCamera.WorldToScreenPoint(position);
                }

                if (gameObject.GetComponent<UnityEngine.RectTransform>() != null)
                {
                    return gameObject.GetComponent<UnityEngine.RectTransform>().position;
                }
                return camera.WorldToScreenPoint(gameObject.transform.position);
            }

            var collider = gameObject.GetComponent<UnityEngine.Collider>();
            if (collider != null)
            {
                position = collider.bounds.center;
            }

            return camera.WorldToScreenPoint(position);
        }
        ///<summary>
        /// Iterate through all cameras until finds one that sees the object.
        /// If no camera sees the object return the position from the last camera
        ///</summary>
        public int findCameraThatSeesObject(UnityEngine.GameObject gameObject, out UnityEngine.Vector3 position)
        {
            position = UnityEngine.Vector3.one * -1;
            int cameraId = -1;
            if (UnityEngine.Camera.allCamerasCount == 0)
            {
                var rectTransform = gameObject.GetComponent<UnityEngine.RectTransform>();
                if (rectTransform != null)
                {
                    var canvas = rectTransform.GetComponentInParent<UnityEngine.Canvas>();
                    if (canvas != null)
                        position = UnityEngine.RectTransformUtility.PixelAdjustPoint(rectTransform.position, rectTransform, canvas.rootCanvas);
                }
                return cameraId;
            }
            foreach (var camera1 in UnityEngine.Camera.allCameras)
            {
                position = getObjectScreenPosition(gameObject, camera1);
                cameraId = camera1.GetInstanceID();
                if (position.x > 0 &&
                    position.y > 0 &&
                    position.x < UnityEngine.Screen.width &&
                    position.y < UnityEngine.Screen.height &&
                    position.z >= 0)//Check if camera sees the object
                {
                    break;
                }
            }
            return cameraId;
        }
        #endregion
    }
}