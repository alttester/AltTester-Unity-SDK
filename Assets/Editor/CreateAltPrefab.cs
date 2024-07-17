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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterUnitySDK.Commands;
using AltTester.AltTesterUnitySDK.InputModule;
using AltTester.AltTesterUnitySDK.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AltTesterTools
{
    public class AltTesterPrefabChecker
    {
        public static bool FloatApproximation(float a, float b, float threshold)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }

        public static void CheckObjectEquality(GameObject originalObject, GameObject newObject)
        {
            if (originalObject.name != newObject.name)
            {
                throw new System.Exception("Object name for: " + originalObject.name + " is different. Original: " + originalObject.name + " and new: " + newObject.name);
            }

            // Check if all components are assigned to object
            var originalObjectComponents = originalObject.GetComponents<Component>();
            var newObjectComponents = newObject.GetComponents<Component>();

            if (originalObjectComponents.Length != newObjectComponents.Length)
            {
                throw new System.Exception("Object components length for: " + originalObject.name + " is different. Original: " + originalObjectComponents.Length + " and new: " + newObjectComponents.Length);
            }

            foreach (var originalComponent in originalObjectComponents)
            {
                if (!Array.Exists(newObjectComponents, component => component.GetType() == originalComponent.GetType()))
                {
                    throw new System.Exception("Object component : " + originalComponent + " for " + originalObject.name + " is not in the new prefab");
                }
            }
            foreach (var newComponent in newObjectComponents)
            {
                if (!Array.Exists(originalObjectComponents, component => component.GetType() == newComponent.GetType()))
                {
                    throw new System.Exception("Object component : " + newComponent + " for " + originalObject.name + " is not in the old prefab");
                }
            }

            foreach (var newComponent in newObjectComponents)
            {
                if (newComponent.GetType() == typeof(RectTransform))
                {
                    checkTransformEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as RectTransform, newComponent as RectTransform);
                    continue;
                }
                if (newComponent.GetType() == typeof(Image))
                {
                    checkImageEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as Image, newComponent as Image);
                    continue;
                }
                if (newComponent.GetType() == typeof(Text))
                {
                    checkTextEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as Text, newComponent as Text);
                    continue;
                }
                if (newComponent.GetType() == typeof(AltDialog))
                {
                    checkAltDialogEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as AltDialog, newComponent as AltDialog);
                    continue;
                }
                if (newComponent.GetType() == typeof(AltRunner))
                {
                    checkAltRunnerEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as AltRunner, newComponent as AltRunner);
                    continue;
                }
            }

            // Check the children if exists
            if (originalObject.transform.childCount != newObject.transform.childCount)
            {
                throw new System.Exception("Object: " + originalObject.name + " has different number of children. Original: " + originalObject.transform.childCount + " and new: " + newObject.transform.childCount);
            }
            for (int i = 0; i < originalObject.transform.childCount; i++)
            {
                CheckObjectEquality(originalObject.transform.GetChild(i).gameObject, newObject.transform.GetChild(i).gameObject);
            }
        }

        private static void checkTransformEquality(RectTransform originalTransform, RectTransform newTransform)
        {
            if (!vector3Equality(originalTransform.position, newTransform.position))
            {
                throw new System.Exception("RectTransform position for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.position + " and new: " + newTransform.position);
            }
            if (!vector3Equality(originalTransform.localPosition, newTransform.localPosition))
            {
                throw new System.Exception("RectTransform localPosition for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.localPosition + " and new: " + newTransform.localPosition);
            }
            if (!vector2Equality(originalTransform.anchorMin, newTransform.anchorMin))
            {
                throw new System.Exception("RectTransform anchorMin for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.anchorMin + " and new: " + newTransform.anchorMin);
            }
            if (!vector2Equality(originalTransform.anchorMax, newTransform.anchorMax))
            {
                throw new System.Exception("RectTransform anchorMax for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.anchorMax + " and new: " + newTransform.anchorMax);
            }
            if (!vector2Equality(originalTransform.anchoredPosition, newTransform.anchoredPosition))
            {
                throw new System.Exception("RectTransform anchoredPosition for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.anchoredPosition + " and new: " + newTransform.anchoredPosition);
            }
            if (!vector2Equality(originalTransform.pivot, newTransform.pivot))
            {
                throw new System.Exception("RectTransform pivot for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.pivot + " and new: " + newTransform.pivot);
            }
            if (!quaternionEquality(originalTransform.rotation, newTransform.rotation))
            {
                throw new System.Exception("RectTransform rotation for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.rotation + " and new: " + newTransform.rotation);
            }
            if (!quaternionEquality(originalTransform.localRotation, newTransform.localRotation))
            {
                throw new System.Exception("RectTransform localRotation for: " + originalTransform.gameObject + " is different. Original: " + originalTransform.localRotation + " and new: " + newTransform.localRotation);
            }
        }

        private static void checkImageEquality(Image originalImage, Image newImage)
        {
            if (originalImage.sprite != newImage.sprite)
            {
                throw new System.Exception("Image sprite for: " + originalImage.gameObject + " is different. Original: " + originalImage.sprite.name + " and new: " + originalImage.sprite.name);
            }
            if (originalImage.color != newImage.color)
            {
                throw new System.Exception("Image color for: " + originalImage.gameObject + " is different. Original: " + originalImage.color + " and new: " + originalImage.color);
            }
            if (originalImage.fillMethod != newImage.fillMethod)
            {
                throw new System.Exception("Image fillMethod for: " + originalImage.gameObject + " is different. Original: " + originalImage.fillMethod + " and new: " + originalImage.fillMethod);
            }
        }

        private static void checkCanvasEquality(Canvas originalCanvas, Canvas newCanvas)
        {
            if (originalCanvas.sortingOrder != newCanvas.sortingOrder)
            {
                throw new System.Exception("SortingOrder for: " + originalCanvas.gameObject + " is different. Original: " + originalCanvas.sortingOrder + " and new: " + newCanvas.sortingOrder);
            }
            if (originalCanvas.renderMode != newCanvas.renderMode)
            {
                throw new System.Exception("RenderMode for: " + originalCanvas.gameObject + " is different. Original: " + originalCanvas.renderMode + " and new: " + newCanvas.renderMode);
            }
        }

        private static void checkTextEquality(Text originalText, Text newText)
        {
            if (originalText.text != newText.text)
            {
                throw new System.Exception("Text for: " + originalText.gameObject + " is different. Original: " + originalText.text + " and new: " + newText.text);
            }
            if (originalText.alignment != newText.alignment)
            {
                throw new System.Exception("Alignment for: " + originalText.gameObject + " is different. Original: " + originalText.alignment + " and new: " + newText.alignment);
            }
            if (originalText.fontSize != newText.fontSize)
            {
                throw new System.Exception("FontSize for: " + originalText.gameObject + " is different. Original: " + originalText.fontSize + " and new: " + newText.fontSize);
            }
            if (originalText.font != newText.font)
            {
                throw new System.Exception("Font for: " + originalText.gameObject + " is different. Original: " + originalText.font + " and new: " + newText.font);
            }
        }

        private static void checkAltDialogEquality(AltDialog originalDialog, AltDialog newDialog)
        {
            if (originalDialog.Dialog.name != newDialog.Dialog.name)
            {
                throw new System.Exception("Dialog object for: " + originalDialog.gameObject + " is different. Original: " + originalDialog.Dialog.name + " and new: " + newDialog.Dialog.name);
            }
            if (originalDialog.TitleText.name != newDialog.TitleText.name)
            {
                throw new System.Exception("TitleText object for: " + originalDialog.gameObject + " is different. Original: " + originalDialog.TitleText.name + " and new: " + newDialog.TitleText.name);
            }
            if (originalDialog.MessageText.name != newDialog.MessageText.name)
            {
                throw new System.Exception("MessageText object for: " + originalDialog.gameObject + " is different. Original: " + originalDialog.MessageText.name + " and new: " + newDialog.MessageText.name);
            }
            if (originalDialog.Icon.name != newDialog.Icon.name)
            {
                throw new System.Exception("Icon object for: " + originalDialog.gameObject + " is different. Original: " + originalDialog.Icon.name + " and new: " + newDialog.Icon.name);
            }
        }

        private static void checkAltRunnerEquality(AltRunner originalRunner, AltRunner newRunner)
        {
            if (originalRunner.outlineShader != newRunner.outlineShader)
            {
                throw new System.Exception("OutlineShader object for: " + originalRunner.gameObject + " is different. Original: " + originalRunner.outlineShader + " and new: " + newRunner.outlineShader);
            }
            if (originalRunner.panelHightlightPrefab.name != newRunner.panelHightlightPrefab.name)
            {
                throw new System.Exception("PanelHightlightPrefab object for: " + originalRunner.gameObject + " is different. Original: " + originalRunner.panelHightlightPrefab.name + " and new: " + newRunner.panelHightlightPrefab.name);
            }
            if (originalRunner.RunOnlyInDebugMode != newRunner.RunOnlyInDebugMode)
            {
                throw new System.Exception("RunOnlyInDebugMode object for: " + originalRunner.gameObject + " is different. Original: " + originalRunner.RunOnlyInDebugMode + " and new: " + newRunner.RunOnlyInDebugMode);
            }
        }

        private static bool vector3Equality(Vector3 originalVector3, Vector3 newVector3)
        {
            return FloatApproximation(originalVector3.x, newVector3.x, 0.01f) && FloatApproximation(originalVector3.y, newVector3.y, 0.01f) && FloatApproximation(originalVector3.z, newVector3.z, 0.01f);
        }

        private static bool vector2Equality(Vector2 originalVector2, Vector2 newVector2)
        {
            return FloatApproximation(originalVector2.x, newVector2.x, 0.01f) && FloatApproximation(originalVector2.y, newVector2.y, 0.01f);
        }

        private static bool quaternionEquality(Quaternion originalQuaternion, Quaternion newQuaternion)
        {
            return FloatApproximation(originalQuaternion.x, newQuaternion.x, 0.01f) && FloatApproximation(originalQuaternion.y, newQuaternion.y, 0.01f) && FloatApproximation(originalQuaternion.z, newQuaternion.z, 0.01f) && FloatApproximation(originalQuaternion.w, newQuaternion.w, 0.01f);
        }
    }

    public class CreateAltPrefab : MonoBehaviour
    {
        public static Font RobotoBold;
        public static Font RobotoRegular;

        public static GameObject CreateAltDialog(Transform parent)
        {
            var AltDialogGameObject = new GameObject("AltDialog", new System.Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(AltDialog) });
            var AltDialogTransform = AltDialogGameObject.GetComponent<RectTransform>();
            AltDialogTransform.SetParent(parent, false);

            AltDialogTransform.localPosition = new Vector3(551, 257.5f, 0);
            AltDialogTransform.anchorMin = Vector2.zero;
            AltDialogTransform.anchorMax = Vector2.zero;
            AltDialogTransform.anchoredPosition = new Vector2(551, 257.5f);
            AltDialogTransform.sizeDelta = new Vector2(2108, 984);
            AltDialogTransform.pivot = new Vector2(0.5f, 0.5f);

            var AltDialogCanvas = AltDialogGameObject.GetComponent<Canvas>();
            AltDialogCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            AltDialogCanvas.sortingOrder = 32767;

            var AltDialogCanvasScaler = AltDialogGameObject.GetComponent<CanvasScaler>();
            AltDialogCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            AltDialogCanvasScaler.referenceResolution = new Vector2(1920, 1080);
            AltDialogCanvasScaler.matchWidthOrHeight = 0.5f;

            return AltDialogGameObject;
        }

        public static GameObject CreateDialog(RectTransform parent)
        {
            var DialogGameObject = new GameObject("Dialog", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
            var DialogTransform = DialogGameObject.GetComponent<RectTransform>();
            DialogTransform.SetParent(parent, false);

            DialogTransform.anchorMin = new Vector2(0.5f, 0.5f);
            DialogTransform.anchorMax = new Vector2(0.5f, 0.5f);
            DialogTransform.pivot = new Vector2(0.5f, 0.5f);
            DialogTransform.sizeDelta = new Vector2(440, 760);
            DialogTransform.localPosition = new Vector3(0, 0, 0);

            var DialogImage = DialogGameObject.GetComponent<Image>();
            DialogImage.color = new Color(0, 0.6470588f, 0.1411765f, 1);
            DialogImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AltTester/Editor/Sprites/Rounded Corners/Rounded20px.png");
            DialogImage.type = Image.Type.Sliced;
            DialogImage.fillCenter = true;
            DialogImage.pixelsPerUnitMultiplier = 1;

            return DialogGameObject;
        }

        public static Text CreateTitle(RectTransform parent)
        {
            var TitleGameObject = new GameObject("Title", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });

            var TitleRectTransform = TitleGameObject.GetComponent<RectTransform>();
            TitleRectTransform.SetParent(parent, false);

            TitleRectTransform.localPosition = new Vector3(0, 365, 0);
            TitleRectTransform.anchorMin = new Vector2(0.5f, 1f);
            TitleRectTransform.anchorMax = new Vector2(0.5f, 1f);
            TitleRectTransform.anchoredPosition = new Vector2(0, -15);
            TitleRectTransform.sizeDelta = new Vector2(440, 65);
            TitleRectTransform.pivot = new Vector2(0.5f, 1f);

            var TitleText = TitleGameObject.GetComponent<Text>();
            TitleText.text = "AltTester®";
            TitleText.fontSize = 27;
            TitleText.font = RobotoBold;
            TitleText.color = Color.white;
            TitleText.alignment = TextAnchor.MiddleCenter;

            return TitleText;
        }

        public static Text CreateStatusMessage(RectTransform parent)
        {
            var MessageGameObject = new GameObject("Message", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });

            var MessageRectTransform = MessageGameObject.GetComponent<RectTransform>();
            MessageRectTransform.SetParent(parent, false);

            MessageRectTransform.localPosition = new Vector3(0, 50f, 0);
            MessageRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            MessageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            MessageRectTransform.anchoredPosition = new Vector2(0, 50f);
            MessageRectTransform.sizeDelta = new Vector2(350, 356);
            MessageRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var MessageText = MessageGameObject.GetComponent<Text>();
            MessageText.text = "Starting communication protocol!";
            MessageText.fontSize = 18;
            MessageText.font = RobotoRegular;

            MessageText.color = Color.white;
            MessageText.alignment = TextAnchor.MiddleCenter;

            return MessageText;
        }

        public static Text CreateInfoLabel(RectTransform parent)
        {
            var LabelGameObject = new GameObject("InfoLabel", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var LabelRectTransform = LabelGameObject.GetComponent<RectTransform>();
            LabelRectTransform.SetParent(parent, false);

            LabelRectTransform.localPosition = new Vector3(0, -262.5f, 0);
            LabelRectTransform.anchoredPosition = new Vector2(0, -262.5f);
            LabelRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            LabelRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            LabelRectTransform.sizeDelta = new Vector2(350, 45);
            LabelRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var LabelText = LabelGameObject.GetComponent<Text>();
            LabelText.text = "To make modifications, enter a new value and press the <b>Restart</b> button.";
            LabelText.fontSize = 18;
            LabelText.font = RobotoRegular;
            LabelText.color = Color.white;
            LabelText.alignment = TextAnchor.MiddleCenter;

            return LabelText;
        }

        public static InputField CreateHostInputField(RectTransform parent)
        {
            var InputFieldGameObject = new GameObject("HostInputField", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(InputField) });

            var InputFieldTransform = InputFieldGameObject.GetComponent<RectTransform>();
            InputFieldTransform.SetParent(parent, false);

            InputFieldTransform.localPosition = new Vector3(-77, -196, 0);
            InputFieldTransform.anchorMin = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchorMax = new Vector2(0.5f, 0.5f);
            InputFieldTransform.sizeDelta = new Vector2(200, 34);
            InputFieldTransform.pivot = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchoredPosition = new Vector2(-77, -196);


            var InputFieldTextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var InputFieldTextTransform = InputFieldTextGameObject.GetComponent<RectTransform>();
            InputFieldTextTransform.SetParent(InputFieldTransform, false);

            InputFieldTextTransform.localPosition = new Vector3(0, -0.5f, 0);
            InputFieldTextTransform.anchoredPosition = new Vector3(0, -0.5f, 0);
            InputFieldTextTransform.sizeDelta = new Vector2(-20, -13);
            InputFieldTextTransform.anchorMin = new Vector2(0, 0);
            InputFieldTextTransform.anchorMax = new Vector2(1, 1);
            InputFieldTextTransform.pivot = new Vector2(0.5f, 0.5f);

            var InputFieldText = InputFieldTextGameObject.GetComponent<Text>();
            InputFieldText.supportRichText = false;
            InputFieldText.fontSize = 17;
            InputFieldText.font = RobotoRegular;
            InputFieldText.color = Color.black;

            var InputFieldPlaceholderGameObject = new GameObject("Placeholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var InputFieldPlaceholderTransform = InputFieldPlaceholderGameObject.GetComponent<RectTransform>();
            InputFieldPlaceholderTransform.SetParent(InputFieldTransform, false);

            InputFieldPlaceholderTransform.localPosition = new Vector3(0, -0.5f, 0);
            InputFieldPlaceholderTransform.sizeDelta = new Vector2(-20, -13);
            InputFieldPlaceholderTransform.anchorMin = new Vector2(0, 0);
            InputFieldPlaceholderTransform.anchorMax = new Vector2(1, 1);
            InputFieldPlaceholderTransform.pivot = new Vector2(0.5f, 0.5f);

            var InputFieldPlaceholder = InputFieldPlaceholderGameObject.GetComponent<Text>();
            InputFieldPlaceholder.supportRichText = false;
            InputFieldPlaceholder.fontSize = 17;
            InputFieldPlaceholder.font = RobotoBold;
            InputFieldPlaceholder.text = "Host...";
            InputFieldPlaceholder.color = Color.gray;

            var InputField = InputFieldGameObject.GetComponent<InputField>();
            InputField.textComponent = InputFieldText;
            InputField.placeholder = InputFieldPlaceholder;

            return InputField;
        }

        public static InputField CreatePortInputField(RectTransform parent)
        {
            var InputFieldGameObject = new GameObject("PortInputField", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(InputField) });

            var InputFieldTransform = InputFieldGameObject.GetComponent<RectTransform>();
            InputFieldTransform.SetParent(parent, false);

            InputFieldTransform.localPosition = new Vector3(102.5f, -198, 0);
            InputFieldTransform.anchorMin = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchorMax = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchoredPosition = new Vector2(102.5f, -196);
            InputFieldTransform.sizeDelta = new Vector2(141, 34);
            InputFieldTransform.pivot = new Vector2(0.5f, 0.5f);

            var InputFieldTextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var InputFieldTextTransform = InputFieldTextGameObject.GetComponent<RectTransform>();
            InputFieldTextTransform.SetParent(InputFieldTransform, false);

            InputFieldTextTransform.localPosition = new Vector3(0, -0.5f, 0);
            InputFieldTextTransform.anchorMin = new Vector2(0, 0);
            InputFieldTextTransform.anchorMax = new Vector2(1, 1);
            InputFieldTextTransform.sizeDelta = new Vector2(-20, -13);
            InputFieldTextTransform.pivot = new Vector2(0.5f, 0.5f);




            var InputFieldText = InputFieldTextGameObject.GetComponent<Text>();
            InputFieldText.supportRichText = false;
            InputFieldText.fontSize = 17;
            InputFieldText.font = RobotoRegular;
            InputFieldText.color = Color.black;

            var InputFieldPlaceholderGameObject = new GameObject("Placeholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var InputFieldPlaceholderTransform = InputFieldPlaceholderGameObject.GetComponent<RectTransform>();
            InputFieldPlaceholderTransform.SetParent(InputFieldTransform, false);

            InputFieldPlaceholderTransform.localPosition = new Vector3(0, -0.5f, 0);
            InputFieldPlaceholderTransform.sizeDelta = new Vector2(-20, -13);
            InputFieldPlaceholderTransform.anchorMin = new Vector2(0, 0);
            InputFieldPlaceholderTransform.anchorMax = new Vector2(1, 1);
            InputFieldPlaceholderTransform.pivot = new Vector2(0.5f, 0.5f);

            var InputFieldPlaceholder = InputFieldPlaceholderGameObject.GetComponent<Text>();
            InputFieldPlaceholder.supportRichText = false;
            InputFieldPlaceholder.fontSize = 17;
            InputFieldPlaceholder.font = RobotoBold;
            InputFieldPlaceholder.text = "Port number...";
            InputFieldPlaceholder.color = Color.gray;

            var InputField = InputFieldGameObject.GetComponent<InputField>();
            InputField.textComponent = InputFieldText;
            InputField.placeholder = InputFieldPlaceholder;

            return InputField;
        }

        public static InputField CreateAppNameInputField(RectTransform parent)
        {
            var InputFieldGameObject = new GameObject("AppNameInputField", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(InputField) });
            var InputFieldTransform = InputFieldGameObject.GetComponent<RectTransform>();
            InputFieldTransform.SetParent(parent, false);



            InputFieldTransform.localPosition = new Vector3(-2, -154, 0);
            InputFieldTransform.anchorMin = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchorMax = new Vector2(0.5f, 0.5f);
            InputFieldTransform.anchoredPosition = new Vector2(-2, -154);
            InputFieldTransform.sizeDelta = new Vector2(350, 34);
            InputFieldTransform.pivot = new Vector2(0.5f, 0.5f);

            var TextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var TextGameObjectTransform = TextGameObject.GetComponent<RectTransform>();
            TextGameObjectTransform.SetParent(InputFieldTransform, false);

            TextGameObjectTransform.localPosition = new Vector3(0, -0.5f, 0);
            TextGameObjectTransform.sizeDelta = new Vector2(-20, -13);
            TextGameObjectTransform.anchorMin = new Vector2(0, 0);
            TextGameObjectTransform.anchorMax = new Vector2(1, 1);
            TextGameObjectTransform.pivot = new Vector2(0.5f, 0.5f);

            var Text = TextGameObject.GetComponent<Text>();
            Text.supportRichText = false;
            Text.fontSize = 17;
            Text.font = RobotoRegular;
            Text.color = Color.black;

            var PlaceholderGameObject = new GameObject("Placeholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var PlaceholderTransform = PlaceholderGameObject.GetComponent<RectTransform>();
            PlaceholderTransform.SetParent(InputFieldTransform, false);

            PlaceholderTransform.localPosition = new Vector3(0, -0.5f, 0);
            PlaceholderTransform.sizeDelta = new Vector2(-20, -13);
            PlaceholderTransform.anchorMin = new Vector2(0, 0);
            PlaceholderTransform.anchorMax = new Vector2(1, 1);
            PlaceholderTransform.pivot = new Vector2(0.5f, 0.5f);

            var Placeholder = PlaceholderGameObject.GetComponent<Text>();
            Placeholder.supportRichText = false;
            Placeholder.fontSize = 17;
            Placeholder.font = RobotoBold;
            Placeholder.text = "App name...";
            Placeholder.color = Color.gray;

            var InputField = InputFieldGameObject.GetComponent<InputField>();
            InputField.textComponent = Text;
            InputField.placeholder = Placeholder;

            return InputField;
        }

        public static Button CreateRestartButton(RectTransform parent)
        {
            var RestartButtonGameObject = new GameObject("RestartButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
            var RestartButtonTransform = RestartButtonGameObject.GetComponent<RectTransform>();
            RestartButtonTransform.SetParent(parent, false);

            RestartButtonTransform.localPosition = new Vector3(0, -320, 0);
            RestartButtonTransform.anchorMin = new Vector2(0.5f, 0.5f);
            RestartButtonTransform.anchorMax = new Vector2(0.5f, 0.5f);
            RestartButtonTransform.sizeDelta = new Vector2(240, 40);
            RestartButtonTransform.pivot = new Vector2(0.5f, 0.5f);

            var RestartButtonTextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var RestartButtonTextTransform = RestartButtonTextGameObject.GetComponent<RectTransform>();
            RestartButtonTextTransform.SetParent(RestartButtonTransform, false);

            RestartButtonTextTransform.localPosition = new Vector3(0, 0, 0);
            RestartButtonTextTransform.anchorMin = new Vector2(0, 0);
            RestartButtonTextTransform.anchorMax = new Vector2(1, 1);
            RestartButtonTextTransform.sizeDelta = new Vector2(0, 0);
            RestartButtonTextTransform.pivot = new Vector2(0.5f, 0.5f);

            var RestartButtonText = RestartButtonTextGameObject.GetComponent<Text>();
            RestartButtonText.text = "Restart";
            RestartButtonText.fontSize = 24;
            RestartButtonText.font = RobotoBold;
            RestartButtonText.color = Color.white;
            RestartButtonText.alignment = TextAnchor.MiddleCenter;

            var RestartButtonImage = RestartButtonGameObject.GetComponent<Image>();
            RestartButtonImage.color = new Color(0, 0.4509804f, 0.09803922f, 1);
            RestartButtonImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AltTester/Editor/Sprites/Rounded Corners/Rounded10px.png");
            RestartButtonImage.type = Image.Type.Sliced;
            RestartButtonImage.fillCenter = true;
            RestartButtonImage.pixelsPerUnitMultiplier = 1;



            return RestartButtonGameObject.GetComponent<Button>();
        }

        public static Button CreateCloseButton(RectTransform parent)
        {
            var CloseButtonGameObject = new GameObject("CloseButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });

            var CloseButtonTransform = CloseButtonGameObject.GetComponent<RectTransform>();
            CloseButtonTransform.SetParent(parent, false);

            CloseButtonTransform.localPosition = new Vector3(205, 365, 0);
            CloseButtonTransform.anchorMin = new Vector2(1f, 1f);
            CloseButtonTransform.anchorMax = new Vector2(1f, 1f);
            CloseButtonTransform.anchoredPosition = new Vector2(-15, -15);
            CloseButtonTransform.sizeDelta = new Vector2(16, 16);
            CloseButtonTransform.pivot = new Vector2(1f, 1f);

            var CloseButtonImage = CloseButtonGameObject.GetComponent<Image>();
            CloseButtonImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Editor/Sprites/XIconWhite.png", typeof(Sprite)) as Sprite;
            CloseButtonImage.SetNativeSize();

            return CloseButtonGameObject.GetComponent<Button>();
        }

        public static Image CreateIcon(RectTransform parent)
        {
            var Icon = new GameObject("Icon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(AltPrefabDrag) });

            var IconTransform = Icon.GetComponent<RectTransform>();
            IconTransform.SetParent(parent, false);

            IconTransform.localPosition = new Vector3(0, 0, 0);
            IconTransform.anchorMin = new Vector2(1f, 0f);
            IconTransform.anchorMax = new Vector2(1f, 0f);
            IconTransform.anchoredPosition = new Vector2(0, 0);
            IconTransform.sizeDelta = new Vector2(100, 100);
            IconTransform.pivot = new Vector2(1f, 0f);

            var IconImage = Icon.GetComponent<Image>();
            IconImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltTester/altTester-512x512.png", typeof(Sprite)) as Sprite;

            return IconImage;
        }

        public static Toggle CreateCustomInputToggle(RectTransform parent)
        {
            var Toggle = new GameObject("Toggle", new System.Type[] { typeof(RectTransform), typeof(Toggle) });

            var ToggleTransform = Toggle.GetComponent<RectTransform>();
            ToggleTransform.SetParent(parent, false);

            ToggleTransform.localPosition = new Vector3(0, -367.5f, 0);
            ToggleTransform.sizeDelta = new Vector2(15, 25);
            ToggleTransform.anchorMin = new Vector2(0.5f, 0.5f);
            ToggleTransform.anchorMax = new Vector2(0.5f, 0.5f);
            ToggleTransform.pivot = new Vector2(0.5f, 0.5f);

            var Background = new GameObject("Background", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
            var BackgroundTransform = Background.GetComponent<RectTransform>();
            BackgroundTransform.SetParent(ToggleTransform, false);

            BackgroundTransform.localPosition = new Vector3(-75.5f, 12.5f, 0);
            BackgroundTransform.sizeDelta = new Vector2(20, 20);
            BackgroundTransform.anchorMin = new Vector2(0, 1);
            BackgroundTransform.anchorMax = new Vector2(0, 1);
            BackgroundTransform.anchoredPosition = new Vector2(-68, 0);
            BackgroundTransform.pivot = new Vector2(0.5f, 0.5f);

            var BackgroundImage = Background.GetComponent<Image>();
            BackgroundImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");

            var CheckMark = new GameObject("Checkmark", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
            var CheckMarkTransform = CheckMark.GetComponent<RectTransform>();
            CheckMarkTransform.SetParent(BackgroundTransform, false);

            CheckMarkTransform.localPosition = new Vector3(0, 0, 0);
            CheckMarkTransform.sizeDelta = new Vector2(20f, 20f);
            CheckMarkTransform.anchorMin = new Vector2(0.5f, 0.5f);
            CheckMarkTransform.anchorMax = new Vector2(0.5f, 0.5f);
            CheckMarkTransform.pivot = new Vector2(0.5f, 0.5f);

            var CheckMarkImage = CheckMark.GetComponent<Image>();
            CheckMarkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");

            var Label = new GameObject("Label", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var LabelTransform = Label.GetComponent<RectTransform>();
            LabelTransform.SetParent(ToggleTransform, false);

            LabelTransform.localPosition = new Vector3(15.25f, 0, 0);
            LabelTransform.sizeDelta = new Vector2(150, 25);
            LabelTransform.anchorMin = new Vector2(0, 1);
            LabelTransform.anchorMax = new Vector2(0, 1);
            LabelTransform.pivot = new Vector2(0.5f, 0.5f);

            var LabelText = Label.GetComponent<Text>();
            LabelText.text = "AltTester® input";
            LabelText.fontSize = 20;
            LabelText.font = RobotoRegular;
            LabelText.alignment = TextAnchor.MiddleLeft;

            var ToggleComponet = Toggle.GetComponent<Toggle>();
            ToggleComponet.targetGraphic = BackgroundImage;
            ToggleComponet.graphic = CheckMarkImage;

            return ToggleComponet;
        }

        public static void SetUpAltRunnerVariables(AltRunner altRunnerComponent)
        {
            var outlineShader = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Runtime/Shader/OutlineShader.shader", typeof(Shader));
            altRunnerComponent.outlineShader = outlineShader as Shader;

            var panelHightlightPrefab = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Runtime/Prefab/Panel.prefab", typeof(GameObject));
            altRunnerComponent.panelHightlightPrefab = panelHightlightPrefab as GameObject;

            altRunnerComponent.RunOnlyInDebugMode = true;
        }

        public static void SavePrefab(GameObject prefab, bool checkEquality = true)
        {
            string Path = "Assets/AltTester/Runtime/Prefab/AltTesterPrefab.prefab";
            string TestPath = "Assets/Editor/AltTesterPrefab.prefab";

            PrefabUtility.SaveAsPrefabAsset(prefab, TestPath);

            var OldPrefab = PrefabUtility.LoadPrefabContents(Path);
            var NewPrefab = PrefabUtility.LoadPrefabContents(TestPath);

            if (checkEquality)
                AltTesterPrefabChecker.CheckObjectEquality(OldPrefab, NewPrefab);

            AssetDatabase.DeleteAsset(Path);

            var message = AssetDatabase.MoveAsset(TestPath, Path);

            if (!String.IsNullOrEmpty(message))
            {
                Debug.LogError(message);
            }
            else
            {
                Debug.Log("Successfully updated AltTesterPrefab.");
            }
        }

        [UnityEditor.MenuItem("AltTester®/Create AltTester® Prefab", false, 80)]
        public static void CreateAltTesterPrefab()
        {
            RobotoBold = AssetDatabase.LoadAssetAtPath<Font>("Assets/AltTester/Resources/Roboto-Bold.ttf");
            RobotoRegular = AssetDatabase.LoadAssetAtPath<Font>("Assets/AltTester/Resources/Roboto-Regular.ttf");
            var prefab = CreatePrefab();
            SavePrefab(prefab);
        }
        [UnityEditor.MenuItem("AltTester®/Create AltTester® Prefab Without Checking Equality", false, 90)]
        public static void CreateAltTesterPrefabWithoutCheck()
        {
            RobotoBold = AssetDatabase.LoadAssetAtPath<Font>("Assets/AltTester/Resources/Roboto-Bold.ttf");
            RobotoRegular = AssetDatabase.LoadAssetAtPath<Font>("Assets/AltTester/Resources/Roboto-Regular.ttf");
            var prefab = CreatePrefab();
            SavePrefab(prefab, false);
        }
        public static GameObject CreatePrefab()
        {
            ///
            /// IMPORTANT! ALTTESTER MUST BE DEFINE TO CREATE CORRECTLY THE PREFAB
            ///

            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTTESTER"))
            {
                Debug.LogError("ALTTESTER must be added as define before updating the prefab.");
            }

            var Prefab = new GameObject("AltTesterPrefab", new System.Type[] { typeof(Transform), typeof(AltRunner), typeof(Input), typeof(NewInputSystem), typeof(CoroutineManager) });
            var RectTransform = Prefab.GetComponent<Transform>();
            var AltRunnerComponent = Prefab.GetComponent<AltRunner>();
            SetUpAltRunnerVariables(AltRunnerComponent);

            var AltDialogGameObject = CreateAltDialog(RectTransform);
            var AltDialogTransform = AltDialogGameObject.GetComponent<RectTransform>();
            var AltDialog = AltDialogGameObject.GetComponent<AltDialog>();

            var Dialog = CreateDialog(AltDialogTransform);
            var DialogTransform = Dialog.GetComponent<RectTransform>();

            AltDialog.Dialog = Dialog;
            AltDialog.InfoArea = CreateInfoArea(DialogTransform);
            var InfoAreaTransform = AltDialog.InfoArea.GetComponent<RectTransform>();
            AltDialog.Icon = CreateIcon(AltDialogTransform);
            AltDialog.TitleText = CreateTitle(DialogTransform);
            AltDialog.SubtitleText = CreateSubtitle(DialogTransform);
            AltDialog.MessageText = CreateStatusMessage(InfoAreaTransform);
            AltDialog.CloseButton = CreateCloseButton(DialogTransform);
            AltDialog.InfoLabel = CreateInfoLabel(DialogTransform);
            AltDialog.HostInputField = CreateHostInputField(InfoAreaTransform);
            AltDialog.PortInputField = CreatePortInputField(InfoAreaTransform);
            AltDialog.AppNameInputField = CreateAppNameInputField(InfoAreaTransform);
            AltDialog.RestartButton = CreateRestartButton(DialogTransform);
            AltDialog.CustomInputToggle = CreateCustomInputToggle(DialogTransform);
            return Prefab;
        }

        private static GameObject CreateInfoArea(RectTransform parent)
        {
            var InfoArea = new GameObject("InfoArea", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });

            var InfoAreaTransform = InfoArea.GetComponent<RectTransform>();
            InfoAreaTransform.SetParent(parent, false);

            InfoAreaTransform.localPosition = new Vector3(0, -2.5f, 0);
            InfoAreaTransform.anchorMin = new Vector2(0.5f, 0.5f);
            InfoAreaTransform.anchorMax = new Vector2(0.5f, 0.5f);
            InfoAreaTransform.anchoredPosition = new Vector2(0, -2.5f);
            InfoAreaTransform.sizeDelta = new Vector2(400, 475);
            InfoAreaTransform.pivot = new Vector2(0.5f, 0.5f);

            var InfoAreaImage = InfoArea.GetComponent<Image>();
            InfoAreaImage.color = new Color(0, 0.4509804f, 0.09803922f, 1);
            InfoAreaImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AltTester/Editor/Sprites/Rounded Corners/Rounded10px.png");
            InfoAreaImage.type = Image.Type.Sliced;
            InfoAreaImage.fillCenter = true;
            InfoAreaImage.pixelsPerUnitMultiplier = 1;


            return InfoArea;
        }

        private static Text CreateSubtitle(RectTransform parent)
        {
            var SubtitleGameObject = new GameObject("Subtitle", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });

            var SubtitleRectTransform = SubtitleGameObject.GetComponent<RectTransform>();
            SubtitleRectTransform.SetParent(parent, false);

            SubtitleRectTransform.localPosition = new Vector3(0, 300, 0);
            SubtitleRectTransform.anchorMin = new Vector2(0.5f, 1f);
            SubtitleRectTransform.anchorMax = new Vector2(0.5f, 1f);
            SubtitleRectTransform.anchoredPosition = new Vector2(0, -80);
            SubtitleRectTransform.sizeDelta = new Vector2(440, 65);
            SubtitleRectTransform.pivot = new Vector2(0.5f, 1f);

            var SubtitleText = SubtitleGameObject.GetComponent<Text>();
            SubtitleText.text = "AltTester®";
            SubtitleText.fontSize = 24;
            SubtitleText.font = RobotoBold;
            SubtitleText.color = Color.white;
            SubtitleText.alignment = TextAnchor.MiddleCenter;

            return SubtitleText;
        }
    }
}
