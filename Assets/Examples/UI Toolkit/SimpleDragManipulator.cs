using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
///  A Manipulator is type of event handler that allows you to control how user interactions affect the
///  UI elements. They provide a flexible way to handle user interactions. Manipulators generally handle
///  a specific type of event, such as mouse clicks, mouse movement, or touch input.
/// </summary>
public class SimpleDragManipulator : MouseManipulator
{
    // Mouse position when mouse button is pressed
    Vector2 m_StartMousePosition;

    // Target element position when mouse button is pressed
    Vector2 m_StartElementPosition;

    // Are we currently dragging this element?
    bool m_IsDragging;

    // Constructor
    public SimpleDragManipulator()
    {
        // This filter determines what condition starts the Manipulator (Left Mouse Button down)
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    // Event subscriptions
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    // Event unsubscriptions
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    // Check if we can click and drag an element and then initialize some values
    private void OnMouseDown(MouseDownEvent evt)
    {
        Debug.Log("Mouse down detected on: " + evt.mousePosition);
        // Checks whether MouseEvent satisfies all of the ManipulatorActivationFilter requirements.
        if (CanStartManipulation(evt))
        {
            // Save the mouse and element start positions
            m_StartMousePosition = evt.mousePosition;
            m_StartElementPosition = new Vector2(target.layout.x, target.layout.y);

            // Flag that this is active and receive all mouse events, even if the mouse pointer leaves
            m_IsDragging = true;
            target.CaptureMouse();

            // Limit the event to this element, do not send up or down the hierarchy
            evt.StopPropagation();
        }
    }

    // Offset the dragged element based on the difference from the start position
    private void OnMouseMove(MouseMoveEvent evt)
    {
        Debug.Log("Mouse Move detected on: " + evt.mousePosition);
        // Checks whether the MouseEvent is related to this Manipulator and dragging is active
        if (CanStopManipulation(evt) && m_IsDragging)
        {
            // Use the difference in mouse position to offset the element as well
            Vector3 mouseDelta = evt.mousePosition - m_StartMousePosition;

            // Convert the pixel offset into a new left/top StyleLength
            target.style.left = CreatePixelLength(m_StartElementPosition.x + mouseDelta.x);
            target.style.top = CreatePixelLength(m_StartElementPosition.y + mouseDelta.y);

            // Limit the event to this element, do not send up or down the hierarchy
            evt.StopPropagation();
        }
    }

    // Creates a new StyleLength representing a length in pixels.
    private StyleLength CreatePixelLength(float value)
    {
        return new StyleLength(new Length(value, LengthUnit.Pixel));
    }

    // Release the element and stop dragging
    private void OnMouseUp(MouseUpEvent evt)
    {
        Debug.Log("Mouse Up detected on: " + evt.mousePosition);
        // Checks whether the MouseEvent is related to this Manipulator and dragging is active
        if (CanStopManipulation(evt) && m_IsDragging)
        {
            // Stop receiving mouse events
            m_IsDragging = false;
            target.ReleaseMouse();

            // Limit the event to this element, do not send up or down the hierarchy
            evt.StopPropagation();
        }
    }
}
