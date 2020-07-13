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
            By cameraBy =(By)System.Enum.Parse(typeof(By), pieces[1]);
            string cameraPath = pieces[2];
            bool enabled = System.Convert.ToBoolean(pieces[3]);
            
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;            
            var path = ProcessPath(objectName);
            var isDirectChild = IsNextElementDirectChild(path[0]);
            var foundGameObject = FindObjects(null, path, 1, true, isDirectChild, enabled);
            UnityEngine.Camera camera = null;
            if (!cameraPath.Equals("//"))
            {
                camera = GetCamera(cameraBy, cameraPath);
                if (camera == null)
                    return AltUnityRunner._altUnityRunner.errorCameraNotFound;
            }
            if (foundGameObject.Count() == 1)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject[0], camera));
            }
            return response;
            
        }
    }
}
