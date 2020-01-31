using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsCommand : AltUnityBaseClassFindObjectsCommand 
    {
        string stringSent;

        public AltUnityFindObjectsCommand (string stringSent)
        {
            this.stringSent = stringSent;
        }

        public override string Execute()
        {
            var pieces = stringSent.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string objectName = pieces[0];
            AltUnityRunner._altUnityRunner.LogMessage("findObjects for: " + objectName);
            string cameraName = pieces[1];
            bool enabled = System.Convert.ToBoolean(pieces[2]);

                UnityEngine.Camera camera = null;
                if (cameraName != null)
                {
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                var path = ProcessPath(objectName);
                var isDirectChild = IsNextElementDirectChild(path[0]);
                    System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
                    foreach (UnityEngine.GameObject testableObject in FindObjects(null, path, 1, false, isDirectChild, enabled))
                    {
                        foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(testableObject, camera));
                    }

                    response = Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);
                return response;
               
        }
    }
}
