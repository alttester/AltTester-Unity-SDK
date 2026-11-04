/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltSetTextCommand : AltReflectionMethodsCommand<AltSetTextParams, AltObject>
    {
        static readonly AltObjectProperty[] textProperties = new[]
        {
            new AltObjectProperty("UnityEngine.UI.Text", "text"),
            new AltObjectProperty("UnityEngine.UI.InputField", "text")
        }.Concat(AltTextMeshPro.TextProperties).ToArray();

        public AltSetTextCommand(AltSetTextParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {

            var targetObject = AltRunner.GetGameObject(CommandParams.altObject.id);

            foreach (var property in textProperties)
            {
                try
                {
                    var type = GetType(property.Component, property.Assembly);
                    string valueText = Newtonsoft.Json.JsonConvert.SerializeObject(CommandParams.value); // Ensure Newtonsoft.Json is correctly referenced
                    SetValueForMember(CommandParams.altObject, property.Property.Split('.'), type, valueText);
                    if (targetObject.TryGetComponent<UnityEngine.UI.InputField>(out var uiInputFieldComp))
                    {

                        uiInputFieldComp.onValueChanged.Invoke(CommandParams.value);
                        checkSubmit(uiInputFieldComp.gameObject);
#if UNITY_2021_1_OR_NEWER
                        uiInputFieldComp.onSubmit.Invoke(CommandParams.value);
#endif
                        uiInputFieldComp.onEndEdit.Invoke(CommandParams.value);
                    }
                    else if (AltTextMeshPro.TryGetInputField(targetObject, out var tMPInputFieldComp))
                    {
                        AltTextMeshPro.GetInputFieldEvent(tMPInputFieldComp, "onValueChanged")?.Invoke(CommandParams.value);
                        checkSubmit(tMPInputFieldComp.gameObject);
                        AltTextMeshPro.GetInputFieldEvent(tMPInputFieldComp, "onSubmit")?.Invoke(CommandParams.value);
                        AltTextMeshPro.GetInputFieldEvent(tMPInputFieldComp, "onEndEdit")?.Invoke(CommandParams.value);
                    }
                    return AltRunner._altRunner.GameObjectToAltObject(targetObject);
                }
                catch (PropertyNotFoundException) { continue; }
                catch (ComponentNotFoundException) { continue; }
                catch (AssemblyNotFoundException) { continue; }
            }
            throw new PropertyNotFoundException("No valid text property could be found or set on the target object.");
        }

        private void checkSubmit(GameObject obj)
        {
            if (CommandParams.submit)
                ExecuteEvents.Execute(obj, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }
}
