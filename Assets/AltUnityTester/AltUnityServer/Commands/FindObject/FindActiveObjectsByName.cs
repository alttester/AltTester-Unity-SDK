using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class FindActiveObjectsByName:Command
    {
        string methodParameters;

        public FindActiveObjectsByName(string stringSent)
        {
            this.methodParameters = stringSent;
        }

        public override string Execute()
        {
            var pieces = methodParameters.Split(new string[] { AltUnityRunner._altUnityRunner.requestSeparatorString }, System.StringSplitOptions.None);
            string objectName = pieces[0];
            UnityEngine.Debug.Log("findActiveObjectByName for: " + objectName);
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
