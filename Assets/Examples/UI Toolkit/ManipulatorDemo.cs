using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManipulatorDemo : MonoBehaviour
{
    const string k_DraggableClass = "draggable";
    List<VisualElement> m_DraggableElements;
    public UIDocument m_Document;
    // Start is called before the first frame update

    protected void Start()
    {
        SetVisualElements();

        SetManipulators();
    }

    // Sets up the VisualElements and stores all draggable elements in the
    // m_DraggableElements list.
    protected void SetVisualElements()
    {

        // Store all elements containing the k_DraggableClass name in a list
        m_DraggableElements = m_Document.rootVisualElement.Query(className: k_DraggableClass).ToList();

    }

    // SetManipulators adds a new instance of SimpleDragManipulator to each draggable
    // element, making it respond to drag-and-drop interactions.

    private void SetManipulators()
    {
        UnityEngine.Debug.Log(m_DraggableElements.Count);
        foreach (VisualElement visualElement in m_DraggableElements)
        {
            visualElement.AddManipulator(new SimpleDragManipulator());
        }
    }
}
