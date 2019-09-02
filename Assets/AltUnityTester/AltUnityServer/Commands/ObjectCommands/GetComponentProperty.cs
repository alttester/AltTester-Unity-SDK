using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetComponentProperty:ReflectionMethods
    {
        string altObjectString;
        string propertyString;

        public GetComponentProperty(string altObjectString, string propertyString)
        {
            this.altObjectString = altObjectString;
            this.propertyString = propertyString;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("get property " + propertyString + " for object " + altObjectString);
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            UnityEngine.GameObject testableObject = AltUnityRunner.GetGameObject(altUnityObject);
            System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
            response = GetValueForMember(memberInfo, testableObject, altProperty);
            return response;
                
        }
    }
}
