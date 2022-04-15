using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Altom.AltUnityTester.Commands
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

                    string valueText = Newtonsoft.Json.JsonConvert.SerializeObject(CommandParams.value);
                    SetValueForMember(CommandParams.altUnityObject, property.Property.Split('.'), type, valueText);
                    var uiInputFieldComp = targetObject.GetComponent<UnityEngine.UI.InputField>();
                    if (uiInputFieldComp != null)
                    {
                        uiInputFieldComp.onValueChanged.Invoke(CommandParams.value);
                        checkSubmit(uiInputFieldComp.gameObject);
                    }
                    else
                    {
                        var tMPInputFieldComp = targetObject.GetComponent<TMPro.TMP_InputField>();
                        if (tMPInputFieldComp != null)
                        {
                            tMPInputFieldComp.onValueChanged.Invoke(CommandParams.value);
                            checkSubmit(tMPInputFieldComp.gameObject);
                            tMPInputFieldComp.onEndEdit.Invoke(CommandParams.value);
                        }
                    }
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

        private void checkSubmit(GameObject obj)
        {
            if (CommandParams.submit)
                ExecuteEvents.Execute(obj, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }
}