using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float minPitch = -40f, maxPitch = 70f;

    Rigidbody rb;
    float yaw, pitch;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        yaw = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity;
        yaw += look.x;
        pitch = Mathf.Clamp(pitch - look.y, minPitch, maxPitch);
        cameraTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }
}