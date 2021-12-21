using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetTextCommand : AltUnityReflectionMethodsCommand<AltUnityGetTextParams, string>
    {
        static readonly AltUnityObjectProperty[] textProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.Text", "text"),
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
            new AltUnityObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        public AltUnityGetTextCommand(AltUnityGetTextParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            Exception exception = null;

            foreach (var property in textProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    return GetValueForMember(CommandParams.altUnityObject, property.Property.Split('.'), type) as string;
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
