using UnityEngine;
using UnityEngine.InputSystem;


namespace Puzventure.Gokturk
{
    /// <summary>
/// Third-person camera that follows the player and rotates with the mouse.
/// Mouse X turns both the camera and the player (yaw).
/// Mouse Y tilts the camera up/down (pitch) without tilting the player.
/// Attach this to your Main Camera and drag your player into "Target".
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The player transform to follow.")]
    public Transform target;
    [Tooltip("Point on the player the camera looks at (e.g. head/shoulder height).")]
    public float lookAtHeight = 1.6f;

    [Header("Distance")]
    public float distance = 4f;
    public float minDistance = 1f;
    public float maxDistance = 8f;
    public float zoomSpeed = 4f;

    [Header("Mouse")]
    [Tooltip("Degrees per pixel of mouse movement.")]
    public float mouseSensitivityX = 0.15f;
    public float mouseSensitivityY = 0.1f;
    public bool invertY = false;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Player Rotation")]
    [Tooltip("If true, the player turns with the mouse.")]
    public bool rotatePlayer = true;

    [Header("Smoothing")]
    [Tooltip("0 = instant follow. Higher = smoother/laggier.")]
    public float followSmoothTime = 0.05f;

    [Header("Collision")]
    [Tooltip("Stops the camera from clipping through walls.")]
    public bool avoidObstacles = true;
    public LayerMask obstacleLayers = ~0;
    public float cameraRadius = 0.25f;

    private float yaw;
    private float pitch;
    private Vector3 followVelocity;
    private Rigidbody targetRb;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("ThirdPersonCamera: No target assigned.");
            enabled = false;
            return;
        }

        targetRb = target.GetComponent<Rigidbody>();

        // Start from the player's current facing direction
        yaw = target.eulerAngles.y;
        pitch = 15f;

        LockCursor(true);
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // Escape unlocks cursor, click locks it again
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) LockCursor(false);
        if (mouse.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked) LockCursor(true);

        if (Cursor.lockState != CursorLockMode.Locked) return;

        // Read mouse movement (pixels this frame)
        Vector2 delta = mouse.delta.ReadValue();
        float mouseX = delta.x * mouseSensitivityX;
        float mouseY = delta.y * mouseSensitivityY;

        yaw += mouseX;
        pitch += invertY ? mouseY : -mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Scroll wheel zoom (one notch in or out per scroll step)
        float scroll = mouse.scroll.ReadValue().y;
        if (scroll != 0f)
        {
            distance -= Mathf.Sign(scroll) * zoomSpeed * 0.25f;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // Turn the player (non-physics player)
        if (rotatePlayer && targetRb == null)
        {
            target.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
    }

    void FixedUpdate()
    {
        // Turn the player (physics player) — Rigidbody should be rotated in FixedUpdate
        if (rotatePlayer && targetRb != null)
        {
            targetRb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + Vector3.up * lookAtHeight;

        // Desired camera position behind the player
        float finalDistance = distance;
        Vector3 direction = rotation * Vector3.back;

        // Pull the camera in if something is between it and the player
        if (avoidObstacles &&
            Physics.SphereCast(focusPoint, cameraRadius, direction, out RaycastHit hit,
                               distance, obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            // Ignore the player's own colliders
            if (!hit.transform.IsChildOf(target))
                finalDistance = Mathf.Max(hit.distance, minDistance * 0.5f);
        }

        Vector3 desiredPosition = focusPoint + direction * finalDistance;

        transform.position = followSmoothTime > 0f
            ? Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothTime)
            : desiredPosition;

        transform.rotation = rotation;
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
}
