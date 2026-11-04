/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetTextCommand : AltReflectionMethodsCommand<AltGetTextParams, string>
    {
        static readonly AltObjectProperty[] textProperties = new[]
        {
            new AltObjectProperty("UnityEngine.UI.Text", "text"),
            new AltObjectProperty("UnityEngine.UI.InputField", "text")
        }.Concat(AltTextMeshPro.TextProperties).ToArray();

        public AltGetTextCommand(AltGetTextParams cmdParams) : base(cmdParams)
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
                    return GetValueForMember(CommandParams.altObject, property.Property.Split('.'), type) as string;
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
        public static string GetText(AltObject altObject)
        {
            var command = new AltGetTextCommand(new AltGetTextParams(altObject));
            return command.Execute();
        }
        public static string GetText(GameObject gameObject)
        {
            if (TryGetText(gameObject, out var text))
            {
                return text;
            }
            throw new PropertyNotFoundException("No valid text property could be found on the target object.");
        }

        /// <summary>
        /// Like GetText, but returns false instead of throwing when the object has no text component.
        /// Use it when checking many objects, e.g. find by text, where throwing for each object without text is slow.
        /// </summary>
        public static bool TryGetText(GameObject gameObject, out string text)
        {
            if (gameObject.TryGetComponent<UnityEngine.UI.Text>(out var uiText))
            {
                text = uiText.text;
                return true;
            }
            if (gameObject.TryGetComponent<UnityEngine.UI.InputField>(out var uiInputField))
            {
                text = uiInputField.text;
                return true;
            }
            return AltTextMeshPro.TryGetText(gameObject, out text);
        }
    }
}
