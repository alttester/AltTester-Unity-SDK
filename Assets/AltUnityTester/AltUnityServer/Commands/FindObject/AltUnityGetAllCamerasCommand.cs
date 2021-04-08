using Altom.AltUnityDriver;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllCamerasCommand : AltUnityCommand
    {
        private readonly bool onlyActiveCameras;
        public AltUnityGetAllCamerasCommand(bool onlyActiveCameras, params string[] parameters) : base(parameters, 2)
        {
            this.onlyActiveCameras = onlyActiveCameras;
        }
        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
            var cameras = Object.FindObjectsOfType<Camera>();
            var cameraObjects = new List<AltUnityObject>();
            if (onlyActiveCameras)
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       where camera.enabled == true
                                       select AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(camera.gameObject));
            }
            else
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       select AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(camera.gameObject));
            }
            response = Newtonsoft.Json.JsonConvert.SerializeObject(cameraObjects);
            return response;
        }
    }
}
