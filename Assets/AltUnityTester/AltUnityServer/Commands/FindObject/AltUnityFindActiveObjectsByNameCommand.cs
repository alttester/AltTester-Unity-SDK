using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindActiveObjectsByNameCommand :AltUnityCommand
    {
        string methodParameters;

        public AltUnityFindActiveObjectsByNameCommand (string stringSent)
        {
            this.methodParameters = stringSent;
        }

        public override string Execute()
        {
            var pieces = methodParameters.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string objectName = pieces[0];
            AltUnityRunner._altUnityRunner.LogMessage("findActiveObjectByName for: " + objectName);
            string cameraName = pieces[1];
            bool enabled = System.Convert.ToBoolean(pieces[2]);
            
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
               
            var foundGameObject = UnityEngine.GameObject.Find(objectName);
            if (foundGameObject != null)
            {
                if (cameraName.Equals(""))
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject));
                else
                {
                    UnityEngine.Camera camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                    response = camera == null ? AltUnityRunner._altUnityRunner.errorNotFoundMessage : Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject, camera));
                }
            }
            return response;
                
        }
    }
}
