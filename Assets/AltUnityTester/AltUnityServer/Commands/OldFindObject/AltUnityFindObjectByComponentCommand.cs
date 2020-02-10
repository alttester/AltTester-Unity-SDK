using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectByComponentCommand :AltUnityReflectionMethodsCommand 
    {
        string methodParameters;

        public AltUnityFindObjectByComponentCommand (string methodParameters)
        {
            this.methodParameters = methodParameters;
        }

        public override string Execute()
        {
            var pieces = methodParameters.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string assemblyName = pieces[0];
            string componentTypeName = pieces[1];
            AltUnityRunner._altUnityRunner.LogMessage("find object by component " + componentTypeName);
            string cameraName = pieces[2];
            bool enabled = System.Convert.ToBoolean(pieces[3]);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                UnityEngine.Camera camera = null;
                if (cameraName != null)
                {
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                System.Type componentType = GetType(componentTypeName, assemblyName);
                if (componentType != null)
                {
                    foreach (UnityEngine.GameObject testableObject in UnityEngine.GameObject.FindObjectsOfType<UnityEngine.GameObject>())
                    {
                        if (testableObject.GetComponent(componentType) != null)
                        {
                            var foundObject = testableObject;
                            response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundObject, camera));
                            break;
                        }
                    }
                }
                else
                {
                    response = AltUnityRunner._altUnityRunner.errorComponentNotFoundMessage;
                }
            return response;
        }
    }
}
