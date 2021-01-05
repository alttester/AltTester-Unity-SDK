using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllCamerasCommand : AltUnityCommand
    {
        public AltUnityGetAllCamerasCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            LogMessage("getAllCameras");
            string response = AltUnityErrors.errorNotFoundMessage;
            var cameras = UnityEngine.GameObject.FindObjectsOfType<UnityEngine.Camera>();
            System.Collections.Generic.List<AltUnityObject> cameraObjects = new System.Collections.Generic.List<AltUnityObject>();
            foreach (UnityEngine.Camera camera in cameras)
            {
                cameraObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(camera.gameObject));
            }
            response = Newtonsoft.Json.JsonConvert.SerializeObject(cameraObjects);
            return response;
        }
    }
}
