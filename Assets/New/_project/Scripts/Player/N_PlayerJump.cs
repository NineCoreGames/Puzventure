using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody rb;
    private InputAction jumpAction;

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
        if (jumpAction.WasPressedThisFrame())
        {
            Debug.Log("SPACE BASILDI!");

            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );

            Debug.Log("ZIPLAMA KUVVETI UYGULANDI!");
        }
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