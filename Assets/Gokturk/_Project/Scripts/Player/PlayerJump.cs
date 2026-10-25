using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    // short wait between jumps, so holding Space doesn't fire twice in one take-off
    [SerializeField] private float jumpCooldown = 0.2f;
    // which layers count as ground: set it to Ground!
    [SerializeField] private LayerMask groundLayers;

    [Header("Coyote Time")]
    // how long after walking off a ledge you can still jump
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    private Rigidbody rb;
    private InputAction jumpAction;
    private float nextJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void FixedUpdate()
    {
        // refill coyote time while grounded, count it down while in the air
        // (the cooldown check stops it refilling in the first frames after a jump)
        if (IsGrounded() && Time.time >= nextJumpTime)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.fixedDeltaTime;

        if (!jumpAction.IsPressed()) return;     // button not held
        if (Time.time < nextJumpTime) return;    // just jumped, wait a moment
        if (coyoteTimeCounter <= 0f) return;     // grounded, or only just left the ground

        // Detach from the moving platform instantly before jumping
        if (transform.parent != null)
            transform.SetParent(null);

        // clear any up/down speed first, so every jump is the same height
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(v.x, 0f, v.z);

        // one short kick upwards
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        nextJumpTime = Time.time + jumpCooldown;

        coyoteTimeCounter = 0f; // used up, so no second jump in mid-air
    }

    // is there Ground right under our feet?
    bool IsGrounded()
    {
        // from our middle, 1.1 m down: half our height + 0.1
        return Physics.Raycast(transform.position, Vector3.down,
                               1.1f, groundLayers);
    }
}