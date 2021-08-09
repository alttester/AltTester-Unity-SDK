using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateAltUnityPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    [UnityEditor.MenuItem("AltUnity Tools/Create AltUnityTester Prefab", false, 80)]
    public static void CreateAUTPrefab()
    {
        ///
        /// IMPORTANT! ALTUNITYTESTER MUST BE DEFINE TO CREATE CORRECTLY THE PREFAB
        ///

        var Prefab = new GameObject("AltUnityRunnerPrefab", new System.Type[] { typeof(Transform), typeof(AltUnityRunner), typeof(Input) });
        bool success;


        string localPath = "Assets/AltUnityTester/Prefab/AltUnityRunnerPrefab.prefab";
        //Set RectTrasnform for rootObject
        var RectTransform = Prefab.GetComponent<Transform>();
        var AltUnityrunner = Prefab.GetComponent<AltUnityRunner>();


        //Create CanvasInputVisualiser
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


        //Create InputVisualiser

        var InputVisualiser = new GameObject("InputVisualiser", new System.Type[] { typeof(RectTransform), typeof(AltUnityInputsVisualiser) });

        var InputVisualiserRectTransform = InputVisualiser.GetComponent<RectTransform>();
        InputVisualiserRectTransform.SetParent(CanvasInputVisualiserRectTransform, false);

        InputVisualiserRectTransform.localPosition = new Vector3(0, 0, 0);
        InputVisualiserRectTransform.anchorMin = Vector2.zero;
        InputVisualiserRectTransform.anchorMax = Vector2.zero;
        InputVisualiserRectTransform.sizeDelta = Vector2.zero;
        InputVisualiserRectTransform.pivot = Vector2.zero;


        var AltUnityInputsVisualiser = InputVisualiser.GetComponent<AltUnityInputsVisualiser>();

        AltUnityInputsVisualiser.VisibleTime = 1;
        AltUnityInputsVisualiser.approachSpeed = 0.02f;
        AltUnityInputsVisualiser.growthBound = 2;

        var InputMark = AssetDatabase.LoadAssetAtPath("Assets/AltUnityTester/Prefab/InputMark.prefab", typeof(GameObject));
        AltUnityInputsVisualiser.Template = ((GameObject)InputMark).GetComponent<AltUnityInputMark>();



        //Create CanvasPopUp

        var CanvasPopUpGameObject = new GameObject("CanvasPopUp", new System.Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) });
        var CanvasPopUpRectTransform = CanvasPopUpGameObject.GetComponent<RectTransform>();
        CanvasPopUpRectTransform.SetParent(RectTransform, false);
        CanvasPopUpRectTransform.localPosition = new Vector3(0, 0, 0);
        CanvasPopUpRectTransform.anchorMin = Vector2.zero;
        CanvasPopUpRectTransform.anchorMax = Vector2.zero;
        CanvasPopUpRectTransform.anchoredPosition = new Vector2(960, 540);
        CanvasPopUpRectTransform.sizeDelta = new Vector2(1920, 1080);
        CanvasPopUpRectTransform.pivot = new Vector2(0.5f, 0.5f);

        var CanvasPopUp = CanvasPopUpGameObject.GetComponent<Canvas>();
        CanvasPopUp.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasPopUp.sortingOrder = 32767;

        var CanvasScalerPopUp = CanvasPopUpGameObject.GetComponent<CanvasScaler>();
        CanvasScalerPopUp.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        CanvasScalerPopUp.referenceResolution = new Vector2(1920, 1080);
        CanvasScalerPopUp.matchWidthOrHeight = 0.5f;


        //Create PopUp

        var PopUp = new GameObject("PopUp", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        var PopUpRectTransform = PopUp.GetComponent<RectTransform>();
        PopUpRectTransform.SetParent(CanvasPopUpRectTransform, false);

        PopUpRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        PopUpRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        PopUpRectTransform.pivot = new Vector2(0.5f, 0.5f);
        PopUpRectTransform.sizeDelta = new Vector2(400, 200);
        PopUpRectTransform.localPosition = new Vector3(0, 0, 0);



        var PopUpImage = PopUp.GetComponent<Image>();
        PopUpImage.color = new Color(0, 0.6470588f, 0.1411765f, 1);

        PopUpImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        PopUpImage.type = Image.Type.Sliced;
        PopUpImage.fillCenter = true;
        PopUpImage.pixelsPerUnitMultiplier = 1;



        //Create AltUnityTesterTitleText

        var VersionTextGameObject = new GameObject("AltUnityTesterTitleText", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
        var VersionTextRectTransform = VersionTextGameObject.GetComponent<RectTransform>();
        VersionTextRectTransform.SetParent(PopUpRectTransform, false);

        VersionTextRectTransform.localPosition = new Vector3(0, 0, 0);
        VersionTextRectTransform.anchorMin = new Vector2(0.5f, 1f);
        VersionTextRectTransform.anchorMax = new Vector2(0.5f, 1f);
        VersionTextRectTransform.sizeDelta = new Vector2(300, 75);
        VersionTextRectTransform.pivot = new Vector2(0.5f, 1f);

        var VersionText = VersionTextGameObject.GetComponent<Text>();
        VersionText.text = "AltUnity Tester v.1.6.5";
        VersionText.fontSize = 30;
        VersionText.color = Color.white;
        VersionText.alignment = TextAnchor.MiddleCenter;


        //Create LookingForConnections

        var LookingForConnections = new GameObject("LookingForConnections", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
        var LookingForConnectionsRectTransform = LookingForConnections.GetComponent<RectTransform>();
        LookingForConnectionsRectTransform.SetParent(PopUpRectTransform, false);

        LookingForConnectionsRectTransform.localPosition = new Vector3(0, 0, 0);
        LookingForConnectionsRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        LookingForConnectionsRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        LookingForConnectionsRectTransform.sizeDelta = new Vector2(400, 75);
        LookingForConnectionsRectTransform.pivot = new Vector2(0.5f, 0.5f);

        var LookingForConnectionsText = LookingForConnections.GetComponent<Text>();
        LookingForConnectionsText.text = "Starting server!";
        LookingForConnectionsText.fontSize = 20;
        LookingForConnectionsText.color = Color.white;
        LookingForConnectionsText.alignment = TextAnchor.MiddleCenter;



        //Create RestartServerButton

        var RestartServerButtonGameObject = new GameObject("RestartServerButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
        var RestartServerButtonRectTransform = RestartServerButtonGameObject.GetComponent<RectTransform>();
        RestartServerButtonRectTransform.SetParent(PopUpRectTransform, false);

        RestartServerButtonRectTransform.localPosition = new Vector3(0, 10, 0);
        RestartServerButtonRectTransform.anchorMin = new Vector2(0.5f, 0f);
        RestartServerButtonRectTransform.anchorMax = new Vector2(0.5f, 0f);
        RestartServerButtonRectTransform.sizeDelta = new Vector2(160, 40);
        RestartServerButtonRectTransform.pivot = new Vector2(0.5f, 0f);

        var RestartServerButton = RestartServerButtonGameObject.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(RestartServerButton.onClick, AltUnityrunner.ServerRestartPressed);



        //Create ButtonText
        var ButtonTextGameObject = new GameObject("LookingForConnections", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });
        var ButtonTextRectTransform = ButtonTextGameObject.GetComponent<RectTransform>();
        ButtonTextRectTransform.SetParent(RestartServerButtonRectTransform, false);

        ButtonTextRectTransform.localPosition = new Vector3(0, 20, 0);
        ButtonTextRectTransform.anchorMin = new Vector2(0f, 0f);
        ButtonTextRectTransform.anchorMax = new Vector2(1f, 1f);
        ButtonTextRectTransform.sizeDelta = new Vector2(0, 0);
        ButtonTextRectTransform.pivot = new Vector2(0.5f, 0.5f);

        var ButtonText = ButtonTextRectTransform.GetComponent<Text>();
        ButtonText.text = "Restart Server";
        ButtonText.fontSize = 15;
        ButtonText.color = Color.black;
        ButtonText.alignment = TextAnchor.MiddleCenter;


        //Create CloseButton

        var CloseButtonGameObject = new GameObject("CloseButton", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
        var CloseButtonRectTransform = CloseButtonGameObject.GetComponent<RectTransform>();
        CloseButtonRectTransform.SetParent(PopUpRectTransform, false);

        CloseButtonRectTransform.localPosition = new Vector3(-15, -15, 0);
        CloseButtonRectTransform.anchorMin = new Vector2(1f, 1f);
        CloseButtonRectTransform.anchorMax = new Vector2(1f, 1f);
        CloseButtonRectTransform.sizeDelta = new Vector2(30, 30);
        CloseButtonRectTransform.pivot = new Vector2(1f, 1f);

        var CloseButton = CloseButtonGameObject.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(CloseButton.onClick, AltUnityrunner.IconPressed);


        var CloseButtonImage = CloseButton.GetComponent<Image>();
        CloseButtonImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltUnityTester/Editor/XIconWhite.png", typeof(Sprite)) as Sprite;
        CloseButtonImage.SetNativeSize();


        //Create Icon

        var Icon = new GameObject("Icon", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button) });
        var IconRectTransform = Icon.GetComponent<RectTransform>();
        IconRectTransform.SetParent(CanvasPopUpRectTransform, false);

        IconRectTransform.localPosition = new Vector3(0, 0, 0);
        IconRectTransform.anchorMin = new Vector2(1f, 0f);
        IconRectTransform.anchorMax = new Vector2(1f, 0f);
        IconRectTransform.anchoredPosition = new Vector2(0, 0);
        IconRectTransform.sizeDelta = new Vector2(100, 100);
        IconRectTransform.pivot = new Vector2(1f, 0f);


        var IconButton = Icon.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(IconButton.onClick, AltUnityrunner.IconPressed);

        var IconImage = Icon.GetComponent<Image>();
        IconImage.sprite = AssetDatabase.LoadAssetAtPath("Assets/AltUnityTester/altUnity-512x512.png", typeof(Sprite)) as Sprite;



        //Set AltUnityRunner variables

        AltUnityrunner.AltUnityPopUp = PopUp;
        AltUnityrunner.AltUnityIcon = IconImage;
        AltUnityrunner.AltUnityPopUpText = LookingForConnectionsText;
        AltUnityrunner.RunOnlyInDebugMode = true;
        var outlineShader = AssetDatabase.LoadAssetAtPath("Assets/AltUnityTester/Shader/OutlineShader.shader", typeof(Shader));
        AltUnityrunner.outlineShader = outlineShader as Shader;
        var panelHightlightPrefab = AssetDatabase.LoadAssetAtPath("Assets/AltUnityTester/Prefab/Panel.prefab", typeof(GameObject));
        AltUnityrunner.panelHightlightPrefab = panelHightlightPrefab as GameObject;
        AltUnityrunner.AltUnityPopUpCanvas = CanvasPopUpGameObject;
        AltUnityrunner.ShowInputs = false;
        AltUnityrunner._inputsVisualiser = InputVisualiser.GetComponent<AltUnityInputsVisualiser>();


        PrefabUtility.SaveAsPrefabAsset(Prefab, localPath, out success);


    }
}
