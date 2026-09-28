using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                moveZ = 1f;

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                moveZ = -1f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveX = -1f;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveX = 1f;
        }

        moveInput = new Vector2(moveX, moveZ).normalized;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        rb.MovePosition(
            rb.position + movement * moveSpeed * Time.fixedDeltaTime
        );
    }
}