using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public UIDocument uIDocument;
    // Start is called before the first frame update
    void Start()
    {
        Label newLabel = new Label("Hello, UI Toolkit!");

        // Set properties for the new label (optional)
        newLabel.style.fontSize = 20;
        newLabel.style.color = Color.red;
        newLabel.name = "Test Space";
        newLabel.style.display = DisplayStyle.None;

        // Add the new label to the root visual element
        var playButton = uIDocument.rootVisualElement.Q<Button>("Play");
        var settingsButton = uIDocument.rootVisualElement.Q<Button>("Settings");
        var quitButton = uIDocument.rootVisualElement.Q<Button>("Quit");
        var toggle = uIDocument.rootVisualElement.Q<Toggle>("Toggle");
        uIDocument.rootVisualElement.Add(newLabel);
        playButton.clicked += () => { UnityEngine.Debug.LogWarning("Play Clicked"); };
        settingsButton.clicked += () => { UnityEngine.Debug.LogWarning("Settings Clicked"); };
        quitButton.clicked += () => { UnityEngine.Debug.LogWarning("Quit Clicked"); };
        toggle.RegisterCallback<ChangeEvent<bool>>(evt =>
        {
            if (evt.newValue)
            {
                // Code to run when the toggle is checked
                Debug.LogWarning("Toggle is now checked!");
            }
            else
            {
                // Code to run when the toggle is unchecked
                Debug.LogWarning("Toggle is now unchecked!");
            }
        });
        // Get the IStyle instance associated with the VisualElement
        IStyle style = playButton.style;

        // Use reflection to get all properties of IStyle
        PropertyInfo[] styleProperties = style.GetType().GetProperties();

        Debug.Log("Listing all inline style properties:");
        foreach (PropertyInfo property in styleProperties)
        {
            try
            {
                // Check if the property value is of type `StyleKeyword`
                var styleValue = property.GetValue(style, null);

                // Only process properties that have been explicitly set (i.e., not undefined)
                if (styleValue is StyleFloat styleFloat && styleFloat.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleInt styleInt && styleInt.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleColor styleColor && styleColor.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleLength styleLength && styleLength.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleEnum<Overflow> styleOverflow && styleOverflow.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleEnum<DisplayStyle> styleDisplay && styleDisplay.keyword != StyleKeyword.Undefined ||
                    styleValue is StyleEnum<FlexDirection> styleFlexDirection && styleFlexDirection.keyword != StyleKeyword.Undefined)
                {
                    Debug.Log($"{property.Name}: {styleValue}");
                }
            }
            catch (System.InvalidCastException)
            {
                // Handle specific cast exceptions
                Debug.LogWarning($"Skipped {property.Name} due to cast issues.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Could not retrieve value for {property.Name}: {e.Message}");
            }
        }
    }


}
