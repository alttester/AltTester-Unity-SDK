using Altom.AltUnityDriver;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllCamerasCommand : AltUnityCommand
    {
        private bool onlyActiveCameras;
        public AltUnityGetAllCamerasCommand(bool onlyActiveCameras, params string[] parameters) : base(parameters, 2)
        {
            this.onlyActiveCameras = onlyActiveCameras;
        }
        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
            var cameras = GameObject.FindObjectsOfType<Camera>();
            List<AltUnityObject> cameraObjects = new List<AltUnityObject>();
            if (onlyActiveCameras)
            {
                LogMessage("getAllActiveCameras");
                cameraObjects.AddRange(from Camera camera in cameras
                                       where camera.enabled == true
                                       select AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(camera.gameObject));
            }
            else
            {
                LogMessage("getAllCameras");
                cameraObjects.AddRange(from Camera camera in cameras
                                       select AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(camera.gameObject));
            }
            response = Newtonsoft.Json.JsonConvert.SerializeObject(cameraObjects);
            return response;
        }
    }
}
