/*
    Copyright(C) 2024 Altom Consulting

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
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltSetTextCommand : AltReflectionMethodsCommand<AltSetTextParams, AltObject>
    {
        static readonly AltObjectProperty[] textProperties =
        {
            new AltObjectProperty("UnityEngine.UI.Text", "text"),
            new AltObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
            new AltObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        public AltSetTextCommand(AltSetTextParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {

            var targetObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            Exception exception = null;

            foreach (var property in textProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    string valueText = Newtonsoft.Json.JsonConvert.SerializeObject(CommandParams.value);
                    SetValueForMember(CommandParams.altObject, property.Property.Split('.'), type, valueText);
                    var uiInputFieldComp = targetObject.GetComponent<UnityEngine.UI.InputField>();
                    if (uiInputFieldComp != null)
                    {
                        uiInputFieldComp.onValueChanged.Invoke(CommandParams.value);
                        checkSubmit(uiInputFieldComp.gameObject);
#if UNITY_2021_1_OR_NEWER
                        uiInputFieldComp.onSubmit.Invoke(CommandParams.value);
#endif
                        uiInputFieldComp.onEndEdit.Invoke(CommandParams.value);
                    }
                    else
                    {
                        var tMPInputFieldComp = targetObject.GetComponent<TMPro.TMP_InputField>();
                        if (tMPInputFieldComp != null)
                        {
                            tMPInputFieldComp.onValueChanged.Invoke(CommandParams.value);
                            checkSubmit(tMPInputFieldComp.gameObject);
                            tMPInputFieldComp.onSubmit.Invoke(CommandParams.value);
                            tMPInputFieldComp.onEndEdit.Invoke(CommandParams.value);
                        }
                    }
                    return AltRunner._altRunner.GameObjectToAltObject(targetObject);
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
