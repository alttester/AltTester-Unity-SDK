using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetObjectComponentProperty:ReflectionMethods
    {
        string altObjectString;
        string propertyString;
        string valueString;

        public SetObjectComponentProperty(string altObjectString, string propertyString, string valueString)
        {
            this.altObjectString = altObjectString;
            this.propertyString = propertyString;
            this.valueString = valueString;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("set property " + propertyString + " to value: " + valueString + " for object " + altObjectString);
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty =
                Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            UnityEngine.GameObject testableObject = AltUnityRunner.GetGameObject(altUnityObject);
            System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
            response = SetValueForMember(memberInfo, valueString, testableObject, altProperty);
            return response;
        }
    }
}
