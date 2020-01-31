using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsWhereNameContainsCommand : AltUnityCommand
    {
        string methodParameters;

        public AltUnityFindObjectsWhereNameContainsCommand (string methodParameters)
        {
            this.methodParameters = methodParameters;
        }

        public override string Execute()
        {
            var pieces = methodParameters.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string objectName = pieces[0];
            AltUnityRunner._altUnityRunner.LogMessage("find objects where name contains:" + objectName);
            string cameraName = pieces[1];
            bool enabled = System.Convert.ToBoolean(pieces[2]);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.Camera camera = null;
            if (cameraName != null)
            {
                camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
            }

            System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
            foreach (UnityEngine.GameObject testableObject in UnityEngine.GameObject.FindObjectsOfType<UnityEngine.GameObject>())
            {
                if (testableObject.name.Contains(objectName))
                {
                    foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(testableObject, camera));
                }
            }

            response = Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);

            return response;
        }
    }
}
