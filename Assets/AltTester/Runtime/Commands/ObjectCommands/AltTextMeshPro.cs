/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Reflection;
using AltTester.AltTesterSDK.Driver;
using UnityEngine;
using UnityEngine.Events;

namespace AltTester.AltTesterUnitySDK.Commands
{
    /// <summary>
    /// TextMeshPro access through reflection, so the same assembly works in projects with and without TextMeshPro
    /// and no TMP_PRESENT scripting define is needed.
    /// </summary>
    static class AltTextMeshPro
    {
        public const string AssemblyName = "Unity.TextMeshPro";
        public const string TextTypeName = "TMPro.TMP_Text";
        public const string InputFieldTypeName = "TMPro.TMP_InputField";

        private const BindingFlags instanceMembers = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly Type textType = Type.GetType($"{TextTypeName}, {AssemblyName}");
        private static readonly Type inputFieldType = Type.GetType($"{InputFieldTypeName}, {AssemblyName}");
        private static readonly PropertyInfo textProperty = textType?.GetProperty("text", BindingFlags.Public | BindingFlags.Instance);
        private static readonly PropertyInfo inputFieldTextProperty = inputFieldType?.GetProperty("text", BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// The TextMeshPro text properties for the get/set text commands, or none when TextMeshPro isn't in the build.
        /// </summary>
        public static readonly AltObjectProperty[] TextProperties = textType == null
            ? new AltObjectProperty[0]
            : new[]
            {
                new AltObjectProperty(TextTypeName, "text", AssemblyName),
                new AltObjectProperty(InputFieldTypeName, "text", AssemblyName)
            };

        public static bool TryGetText(GameObject gameObject, out string text)
        {
            text = null;
            if (textProperty != null && gameObject.TryGetComponent(textType, out var textComponent))
            {
                text = textProperty.GetValue(textComponent) as string;
                return true;
            }
            if (inputFieldTextProperty != null && gameObject.TryGetComponent(inputFieldType, out var inputField))
            {
                text = inputFieldTextProperty.GetValue(inputField) as string;
                return true;
            }
            return false;
        }

        public static bool TryGetInputField(GameObject gameObject, out Component inputField)
        {
            inputField = null;
            return inputFieldType != null && gameObject.TryGetComponent(inputFieldType, out inputField);
        }

        /// <summary>
        /// Returns a TMP_InputField event such as onValueChanged, onSubmit or onEndEdit, or null if it isn't found.
        /// </summary>
        public static UnityEvent<string> GetInputFieldEvent(Component inputField, string eventName)
        {
            var type = inputField.GetType();
            var property = type.GetProperty(eventName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
                return property.GetValue(inputField) as UnityEvent<string>;
            // The serialized backing field, e.g. m_OnValueChanged, in case the property accessor was stripped.
            var field = type.GetField("m_" + char.ToUpperInvariant(eventName[0]) + eventName.Substring(1), instanceMembers);
            return field?.GetValue(inputField) as UnityEvent<string>;
        }
    }
}
