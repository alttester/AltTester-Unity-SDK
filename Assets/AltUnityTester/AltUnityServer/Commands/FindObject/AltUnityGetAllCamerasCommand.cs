using Altom.AltUnityDriver;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllCamerasCommand : AltUnityCommand<CommandParams, List<AltUnityObject>>
    {
        private readonly bool onlyActiveCameras;

        public AltUnityGetAllCamerasCommand(AltUnityGetAllCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = false;
        }
        public AltUnityGetAllCamerasCommand(AltUnityGetAllActiveCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = true;
        }
        public override List<AltUnityObject> Execute()
        {
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
            return cameraObjects;
        }
    }
}
