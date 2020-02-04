using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectCommand :AltUnityBaseClassFindObjectsCommand 
    {
        string stringSent;

        public AltUnityFindObjectCommand (string stringSent)
        {
            this.stringSent = stringSent;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("findObject for: " + stringSent);
            var pieces = stringSent.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string objectName = pieces[0];
            string cameraName = pieces[1];
            bool enabled = System.Convert.ToBoolean(pieces[2]);
            
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;            
            var path = ProcessPath(objectName);
            var isDirectChild = IsNextElementDirectChild(path[0]);
            var foundGameObject = FindObjects(null, path, 1, true, isDirectChild, enabled);
            if (foundGameObject.Count() == 1)
            {
                if (cameraName.Equals(""))
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject[0]));
                else
                {
                    UnityEngine.Camera camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                    response = camera == null ? AltUnityRunner._altUnityRunner.errorNotFoundMessage : Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject[0], camera));
                }
            }
            return response;
            
        }
    }
}
