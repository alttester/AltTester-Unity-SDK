using UnityEngine;
using UnityEngine.InputSystem;

public class AltFirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 100f;

    [Header("Input Mode")]
    [Tooltip("Use raw mouse position delta instead of Mouse.current.delta for mouse look.")]
    public bool useMouseDelta = false;

    [Header("Camera Reference")]
    public Transform playerBody; // Assign the Player (root) transform

    private float xRotation = 0f;
    private bool isLocked = false;
    private Vector2 lastMousePosition;

    private Mouse mouse;
    private Keyboard keyboard;

    void Start()
    {
        mouse = Mouse.current;
        keyboard = Keyboard.current;

        // Start unlocked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        mouse = Mouse.current;
        keyboard = Keyboard.current;

        if (mouse == null || keyboard == null) return;

        // Toggle lock/unlock on L key
        if (keyboard.lKey.wasPressedThisFrame)
        {
            isLocked = !isLocked;

            if (isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                lastMousePosition = mouse.position.ReadValue();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // Only move camera when locked
        if (!isLocked) return;

        float mouseX, mouseY;

        if (useMouseDelta)
        {
            Vector2 currentMousePosition = mouse.position.ReadValue();
            Vector2 delta = currentMousePosition - lastMousePosition;
            lastMousePosition = currentMousePosition;

            mouseX = delta.x * mouseSensitivity * Time.deltaTime;
            mouseY = delta.y * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            Vector2 delta = mouse.delta.ReadValue();
            mouseX = delta.x * mouseSensitivity * Time.deltaTime;
            mouseY = delta.y * mouseSensitivity * Time.deltaTime;
        }

        // Vertical rotation (clamp to avoid flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply vertical rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply horizontal rotation to player body
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}
