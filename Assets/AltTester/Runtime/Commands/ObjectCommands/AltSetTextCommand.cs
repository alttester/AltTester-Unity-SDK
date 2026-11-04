/*
    Copyright(C) 2026 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
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
