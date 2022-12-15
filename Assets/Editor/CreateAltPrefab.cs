using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Altom.AltTester;
using Altom.AltTester.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Altom.AltTesterTools
{
    public class CreateAltPrefab : MonoBehaviour
    {

        // Start is called before the first frame update
        [UnityEditor.MenuItem("AltTester/Create AltTester Prefab", false, 80)]
        public static void CreateAUTPrefab()
        {
            ///
            /// IMPORTANT! ALTTESTER MUST BE DEFINE TO CREATE CORRECTLY THE PREFAB
            ///

            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTTESTER"))
            {
                Debug.LogError("ALTTESTER must be added as define before updating the prefab");
            }

            var Prefab = new GameObject("AltTesterPrefab", new System.Type[] { typeof(Transform), typeof(AltRunner), typeof(Input), typeof(NewInputSystem) });

            string path = "Assets/AltTester/Prefab/AltTesterPrefab.prefab";
            string localPath = path;


            // Set RectTransform for rootObject
            var RectTransform = Prefab.GetComponent<Transform>();
            var Altrunner = Prefab.GetComponent<AltRunner>();


            // Create CanvasInputVisualiser
            var CanvasInputVisualiserGameObject = new GameObject("CanvasInputVisualiser", new System.Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) });
            var CanvasInputVisualiserRectTransform = CanvasInputVisualiserGameObject.GetComponent<RectTransform>();

            CanvasInputVisualiserRectTransform.SetParent(RectTransform, false);
            CanvasInputVisualiserRectTransform.localPosition = new Vector3(0, 0, 0);
            CanvasInputVisualiserRectTransform.anchorMin = Vector2.zero;
            CanvasInputVisualiserRectTransform.anchorMax = Vector2.zero;
            CanvasInputVisualiserRectTransform.anchoredPosition = new Vector2(960, 540);
            CanvasInputVisualiserRectTransform.sizeDelta = new Vector2(1920, 1080);
            CanvasInputVisualiserRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var CanvasInputVisualiser = CanvasInputVisualiserGameObject.GetComponent<Canvas>();
            CanvasInputVisualiser.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasInputVisualiser.sortingOrder = 32767;


            // Create InputVisualiser
            var InputVisualiser = new GameObject("InputVisualiser", new System.Type[] { typeof(RectTransform), typeof(AltInputsVisualizer) });

            var InputVisualiserRectTransform = InputVisualiser.GetComponent<RectTransform>();
            InputVisualiserRectTransform.SetParent(CanvasInputVisualiserRectTransform, false);

            InputVisualiserRectTransform.localPosition = new Vector3(0, 0, 0);
            InputVisualiserRectTransform.anchorMin = Vector2.zero;
            InputVisualiserRectTransform.anchorMax = Vector2.zero;
            InputVisualiserRectTransform.sizeDelta = Vector2.zero;
            InputVisualiserRectTransform.pivot = Vector2.zero;


            var AltInputsVisualiser = InputVisualiser.GetComponent<AltInputsVisualizer>();

            AltInputsVisualiser.VisibleTime = 1;
            AltInputsVisualiser.approachSpeed = 0.02f;
            AltInputsVisualiser.growthBound = 2;

            var InputMark = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Prefab/InputMark.prefab", typeof(GameObject));
            AltInputsVisualiser.Template = ((GameObject)InputMark).GetComponent<AltInputMark>();



            // Create AltDialog
            var AltDialogGameObject = new GameObject("AltDialog", new System.Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(AltDialog) });
            var AltDialogTransform = AltDialogGameObject.GetComponent<RectTransform>();
            AltDialogTransform.SetParent(RectTransform, false);
            AltDialogTransform.localPosition = new Vector3(0, 0, 0);
            AltDialogTransform.anchorMin = Vector2.zero;
            AltDialogTransform.anchorMax = Vector2.zero;
            AltDialogTransform.anchoredPosition = new Vector2(960, 540);
            AltDialogTransform.sizeDelta = new Vector2(3135, 661);
            AltDialogTransform.pivot = new Vector2(0.5f, 0.5f);

            var AltDialogCanvas = AltDialogGameObject.GetComponent<Canvas>();
            AltDialogCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            AltDialogCanvas.sortingOrder = 32767;

            var AltDialogCanvasScaler = AltDialogGameObject.GetComponent<CanvasScaler>();
            AltDialogCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            AltDialogCanvasScaler.referenceResolution = new Vector2(1920, 1080);
            AltDialogCanvasScaler.matchWidthOrHeight = 0.5f;


            var AltDialog = AltDialogGameObject.GetComponent<AltDialog>();


            // Create Dialog
            var DialogGameObject = new GameObject("Dialog", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
            var DialogTransform = DialogGameObject.GetComponent<RectTransform>();
            DialogTransform.SetParent(AltDialogTransform, false);

            DialogTransform.anchorMin = new Vector2(0.5f, 0.5f);
            DialogTransform.anchorMax = new Vector2(0.5f, 0.5f);
            DialogTransform.pivot = new Vector2(0.5f, 0.5f);
            DialogTransform.sizeDelta = new Vector2(440, 440);
            DialogTransform.localPosition = new Vector3(0, 0, 0);

            var DialogImage = DialogGameObject.GetComponent<Image>();
            DialogImage.color = new Color(0, 0.6470588f, 0.1411765f, 1);
            DialogImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            DialogImage.type = Image.Type.Sliced;
            DialogImage.fillCenter = true;
            DialogImage.pixelsPerUnitMultiplier = 1;


            // Create AltTesterTitleText
            var TitleGameObject = new GameObject("Title", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var TitleRectTransform = TitleGameObject.GetComponent<RectTransform>();
            TitleRectTransform.SetParent(DialogTransform, false);

            TitleRectTransform.localPosition = new Vector3(0, -31, 0);
            TitleRectTransform.anchorMin = new Vector2(0.5f, 1f);
            TitleRectTransform.anchorMax = new Vector2(0.5f, 1f);
            TitleRectTransform.sizeDelta = new Vector2(300, 75);
            TitleRectTransform.pivot = new Vector2(0.5f, 1f);

            var TitleText = TitleGameObject.GetComponent<Text>();
            TitleText.text = "AltTester v.1.8.1";
            TitleText.fontSize = 30;
            TitleText.color = Color.white;
            TitleText.alignment = TextAnchor.MiddleCenter;

            // Create LookingForConnections
            var MessageGameObject = new GameObject("Message", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var MessageRectTransform = MessageGameObject.GetComponent<RectTransform>();
            MessageRectTransform.SetParent(DialogTransform, false);

            MessageRectTransform.localPosition = new Vector3(0, 60, 0);
            MessageRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            MessageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            MessageRectTransform.sizeDelta = new Vector2(400, 100);
            MessageRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var MessageText = MessageGameObject.GetComponent<Text>();
            MessageText.text = "Starting communication protocol!";
            MessageText.fontSize = 24;
            MessageText.color = Color.white;
            MessageText.alignment = TextAnchor.MiddleCenter;

            // Create Port Label
            var PortLabel = new GameObject("PortLabel", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var PortLabelRectTransform = PortLabel.GetComponent<RectTransform>();
            PortLabelRectTransform.SetParent(DialogTransform, false);

            PortLabelRectTransform.localPosition = new Vector3(0, -50, 0);
            PortLabelRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            PortLabelRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            PortLabelRectTransform.sizeDelta = new Vector2(400, 75);
            PortLabelRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var PortLabelText = PortLabel.GetComponent<Text>();
            PortLabelText.text = "To change the port number input a new port and press the Restart button.";
            PortLabelText.fontSize = 20;
            PortLabelText.color = Color.white;
            PortLabelText.alignment = TextAnchor.MiddleCenter;

            // Create Port Input Field
            var PortInputFieldGameObject = new GameObject("PortInputField", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(InputField) });
            var PortInputFieldTransform = PortInputFieldGameObject.GetComponent<RectTransform>();
            PortInputFieldTransform.SetParent(DialogTransform, false);

            PortInputFieldTransform.localPosition = new Vector3(0, -110, 0);
            PortInputFieldTransform.anchorMin = new Vector2(0.5f, 0.5f);
            PortInputFieldTransform.anchorMax = new Vector2(0.5f, 0.5f);
            PortInputFieldTransform.sizeDelta = new Vector2(240, 34);
            PortInputFieldTransform.pivot = new Vector2(0.5f, 0.5f);

            var PortInputFieldTextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var PortInputFieldTextTransform = PortInputFieldTextGameObject.GetComponent<RectTransform>();
            PortInputFieldTextTransform.SetParent(PortInputFieldTransform, false);

            PortInputFieldTextTransform.localPosition = new Vector3(0, -0.5f, 0);
            PortInputFieldTextTransform.sizeDelta = new Vector2(-20, -13);
            PortInputFieldTextTransform.anchorMin = new Vector2(0, 0);
            PortInputFieldTextTransform.anchorMax = new Vector2(1, 1);
            PortInputFieldTextTransform.pivot = new Vector2(0.5f, 0.5f);

            var PortInputFieldText = PortInputFieldTextGameObject.GetComponent<Text>();
            PortInputFieldText.supportRichText = false;
            PortInputFieldText.fontSize = 18;
            PortInputFieldText.color = Color.black;

            var PortInputFieldPlaceholderGameObject = new GameObject("Placeholder", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var PortInputFieldPlaceholderTransform = PortInputFieldPlaceholderGameObject.GetComponent<RectTransform>();
            PortInputFieldPlaceholderTransform.SetParent(PortInputFieldTransform, false);

            PortInputFieldPlaceholderTransform.localPosition = new Vector3(0, -0.5f, 0);
            PortInputFieldPlaceholderTransform.sizeDelta = new Vector2(-20, -13);
            PortInputFieldPlaceholderTransform.anchorMin = new Vector2(0, 0);
            PortInputFieldPlaceholderTransform.anchorMax = new Vector2(1, 1);
            PortInputFieldPlaceholderTransform.pivot = new Vector2(0.5f, 0.5f);

            var PortInputFieldPlaceholder = PortInputFieldPlaceholderGameObject.GetComponent<Text>();
            PortInputFieldPlaceholder.supportRichText = false;
            PortInputFieldPlaceholder.fontSize = 18;
            PortInputFieldPlaceholder.text = "Enter port number...";
            PortInputFieldPlaceholder.color = Color.gray;

            var PortInputField = PortInputFieldGameObject.GetComponent<InputField>();
            PortInputField.textComponent = PortInputFieldText;
            PortInputField.placeholder = PortInputFieldPlaceholder;

            // Create Restart Button
            var RestartButtonGameObject = new GameObject("RestartButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
            var RestartButtonRectTransform = RestartButtonGameObject.GetComponent<RectTransform>();
            RestartButtonRectTransform.SetParent(DialogTransform, false);

            RestartButtonRectTransform.localPosition = new Vector3(0, -155, 0);
            RestartButtonRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            RestartButtonRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            RestartButtonRectTransform.sizeDelta = new Vector2(240, 34);
            RestartButtonRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var RestartButtonTextGameObject = new GameObject("Text", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
            var RestartButtonTextRectTransform = RestartButtonTextGameObject.GetComponent<RectTransform>();
            RestartButtonTextRectTransform.SetParent(RestartButtonRectTransform, false);

            RestartButtonTextRectTransform.localPosition = new Vector3(0, 0, 0);
            RestartButtonTextRectTransform.anchorMin = new Vector2(0, 0);
            RestartButtonTextRectTransform.anchorMax = new Vector2(1, 1);
            RestartButtonTextRectTransform.sizeDelta = new Vector2(0, 0);
            RestartButtonTextRectTransform.pivot = new Vector2(0.5f, 0.5f);

            var RestartButtonText = RestartButtonTextGameObject.GetComponent<Text>();
            RestartButtonText.text = "Restart";
            RestartButtonText.fontSize = 18;
            RestartButtonText.color = Color.black;
            RestartButtonText.alignment = TextAnchor.MiddleCenter;

            var RestartButton = RestartButtonGameObject.GetComponent<Button>();

            // Create CloseButton
            var CloseButtonGameObject = new GameObject("CloseButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
            var CloseButtonRectTransform = CloseButtonGameObject.GetComponent<RectTransform>();
            CloseButtonRectTransform.SetParent(DialogTransform, false);

            CloseButtonRectTransform.localPosition = new Vector3(-15, -15, 0);
            CloseButtonRectTransform.anchorMin = new Vector2(1f, 1f);
            CloseButtonRectTransform.anchorMax = new Vector2(1f, 1f);
            CloseButtonRectTransform.sizeDelta = new Vector2(30, 30);
            CloseButtonRectTransform.pivot = new Vector2(1f, 1f);

            var CloseButtonImage = CloseButtonGameObject.GetComponent<Image>();
            CloseButtonImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Editor/XIconWhite.png", typeof(Sprite)) as Sprite;
            CloseButtonImage.SetNativeSize();
            var CloseButton = CloseButtonGameObject.GetComponent<Button>();


            // Create Icon
            var Icon = new GameObject("Icon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(AltPrefabDrag) });
            var IconRectTransform = Icon.GetComponent<RectTransform>();
            IconRectTransform.SetParent(AltDialogTransform, false);

            IconRectTransform.localPosition = new Vector3(0, 0, 0);
            IconRectTransform.anchorMin = new Vector2(1f, 0f);
            IconRectTransform.anchorMax = new Vector2(1f, 0f);
            IconRectTransform.anchoredPosition = new Vector2(0, 0);
            IconRectTransform.sizeDelta = new Vector2(100, 100);
            IconRectTransform.pivot = new Vector2(1f, 0f);

            var IconImage = Icon.GetComponent<Image>();
            IconImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltTester/altTester-512x512.png", typeof(Sprite)) as Sprite;


            // Set AltRunner variables
            var outlineShader = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Shader/OutlineShader.shader", typeof(Shader));
            Altrunner.outlineShader = outlineShader as Shader;
            var panelHightlightPrefab = AssetDatabase.LoadAssetAtPath("Assets/AltTester/Prefab/Panel.prefab", typeof(GameObject));
            Altrunner.panelHightlightPrefab = panelHightlightPrefab as GameObject;
            Altrunner.RunOnlyInDebugMode = true;
            Altrunner.InputsVisualizer = AltInputsVisualiser;


            // Set AltDialog variables
            AltDialog.Dialog = DialogGameObject;
            AltDialog.TitleText = TitleText;
            AltDialog.MessageText = MessageText;
            AltDialog.CloseButton = CloseButton;
            AltDialog.Icon = IconImage;
            AltDialog.PortLabel = PortLabelText;
            AltDialog.PortInputField = PortInputField;
            AltDialog.RestartButton = RestartButton;

            var testPath = "Assets/Editor/AltRunnerPrefab.prefab";
            PrefabUtility.SaveAsPrefabAsset(Prefab, testPath);


            var oldPrefab = PrefabUtility.LoadPrefabContents(path);
            var newPrefab = PrefabUtility.LoadPrefabContents(testPath);
            checkObjectEquality(oldPrefab, newPrefab);
            AssetDatabase.DeleteAsset(path);
            var message = AssetDatabase.MoveAsset(testPath, path);
            if (!String.IsNullOrEmpty(message))
            {
                Debug.LogError(message);
            }
            else
            {
                Debug.Log("Successfully updated AltTesterPrefab");
            }
        }

        private static void checkObjectEquality(GameObject originalObject, GameObject newObject)
        {
            if (originalObject.name != newObject.name)
            {
                throw new System.Exception("Object name for: " + originalObject.name + " is different. Original: " + originalObject.name + " and new: " + newObject.name);
            }

            //Check if all components are assigned to object
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
                    checkTranformEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as RectTransform, newComponent as RectTransform);
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
                    checkaltRunnerEquality(originalObjectComponents.First(Component => Component.GetType() == newComponent.GetType()) as AltRunner, newComponent as AltRunner);
                    continue;
                }
            }

            //Check the children if exists

            if (originalObject.transform.childCount != newObject.transform.childCount)
            {
                throw new System.Exception("Object: " + originalObject.name + " has different number of children. Original: " + originalObject.transform.childCount + " and new: " + newObject.transform.childCount);
            }
            for (int i = 0; i < originalObject.transform.childCount; i++)
            {
                checkObjectEquality(originalObject.transform.GetChild(i).gameObject, newObject.transform.GetChild(i).gameObject);
            }
        }
        private static void checkTranformEquality(RectTransform originalTransform, RectTransform newTransform)
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
        private static void checkaltRunnerEquality(AltRunner originalRunner, AltRunner newRunner)
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

        public static bool FloatApproximation(float a, float b, float threshold)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }
    }
}