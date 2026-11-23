using UnityEngine;
using UnityEngine.InputSystem;

public class N_PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        jumpAction = new InputAction(
            "Jump",
            InputActionType.Button,
            "<Keyboard>/space"
        );
    }

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void Update()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            isGrounded = false;

            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(
                Physics.gravity * 2f,
                ForceMode.Acceleration
            );
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }

    private void OnDestroy()
    {
        jumpAction.Dispose();
    }
}