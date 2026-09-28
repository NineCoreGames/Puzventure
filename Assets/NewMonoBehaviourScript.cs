using UnityEngine;
// 1. MUST ADD THIS LINE AT THE TOP
using UnityEngine.InputSystem; 

public class CapsuleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput; // Changed to Vector2 for the new system

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 2. Uses the New Input System's built-in WASD/Arrow detection
        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveZ = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveZ = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;

            moveInput = new Vector2(moveX, moveZ).normalized;
        }
    }

    void FixedUpdate()
    {
        // Convert the 2D input into a 3D movement vector
        Vector3 moveVelocity = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
