/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetAllCamerasCommand : AltCommand<CommandParams, List<AltObject>>
    {
        private readonly bool onlyActiveCameras;

        public AltGetAllCamerasCommand(AltGetAllCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = false;
        }
        public AltGetAllCamerasCommand(AltGetAllActiveCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = true;
        }
        public override List<AltObject> Execute()
        {
#if UNITY_6000_4_OR_NEWER
            var cameras = Object.FindObjectsByType<Camera>();
#elif UNITY_6000_0_OR_NEWER
            var cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
#else
            var cameras = Object.FindObjectsOfType<Camera>();
#endif
            var cameraObjects = new List<AltObject>();
            if (onlyActiveCameras)
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       where camera.enabled == true
                                       select AltRunner._altRunner.GameObjectToAltObject(camera.gameObject));
            }
            else
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       select AltRunner._altRunner.GameObjectToAltObject(camera.gameObject));
            }
            return cameraObjects;
        }
    }
}
