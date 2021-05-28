using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTextCommand : AltUnityReflectionMethodsCommand<AltUnitySetTextParams, AltUnityObject>
    {
        static readonly AltUnityObjectProperty[] textProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.Text", "text"),
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
            new AltUnityObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        public AltUnitySetTextCommand(AltUnitySetTextParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var targetObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            Exception exception = null;

            foreach (var property in textProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    SetValueForMember(CommandParams.altUnityObject, property.Property.Split('.'), type, CommandParams.value);
                    return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(targetObject);
                }
                catch (PropertyNotFoundException ex)
                {
                    exception = ex;
                }
                catch (ComponentNotFoundException ex)
                {
                    exception = ex;
                }
                catch (AssemblyNotFoundException ex)
                {
                    exception = ex;
                }
            }
            if (exception != null) throw exception;
            throw new Exception("Something went wrong"); // should not reach this point
        }
    }
}