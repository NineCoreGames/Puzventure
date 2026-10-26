using UnityEngine;
using UnityEngine.InputSystem;


namespace Puzventure.Gokturk
{
    public class PlayerMove : MonoBehaviour
{
    // the knobs of movement, shown in the Inspector
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 40f;

    [Header("Ground check")]
    [SerializeField] private float groundCheckExtra = 0.1f; // how far below the feet we look
    [SerializeField] private LayerMask groundLayers = ~0;

    private Rigidbody rb;
    private Collider col;
    private InputAction moveAction;
    private Vector2 input;   // raw WASD / stick
    private float grip = 1f; // 1 = normal ground

    // the thing we are standing on, and where it was last physics step
    private Transform ground;
    private Vector3 groundLastPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // every frame: read WASD, arrows or the left stick
        input = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // 1) what are we standing on, and how fast is it moving?
        Vector3 platformVel = GetGroundVelocity();

        // 2) turn the input into the player's own directions
        Vector3 direction = rb.rotation * new Vector3(input.x, 0f, input.y);
        Vector3 wanted = direction * maxSpeed;

        // 3) our speed compared to the ground under us (not up/down)
        Vector3 v = rb.linearVelocity;
        Vector3 flat = new Vector3(v.x, 0f, v.z);
        Vector3 flatPlatform = new Vector3(platformVel.x, 0f, platformVel.z);
        Vector3 relative = flat - flatPlatform;

        // get a little closer to the wanted speed (relative to the ground)
        float step = acceleration * grip * Time.fixedDeltaTime;
        relative = Vector3.MoveTowards(relative, wanted, step);

        // add the platform's motion back on top, so it carries us
        flat = relative + flatPlatform;

        // if the platform is going down, go down with it instead of bouncing
        if (platformVel.y < 0f) v.y = Mathf.Min(v.y, platformVel.y);

        rb.linearVelocity = new Vector3(flat.x, v.y, flat.z);

        // no turning here — the camera script turns the player
    }

    // Looks just below the feet. Returns how fast the thing under us moves.
    private Vector3 GetGroundVelocity()
    {
        Bounds b = col.bounds;
        float radius = Mathf.Min(b.extents.x, b.extents.z) * 0.9f;
        float castDistance = b.extents.y - radius + groundCheckExtra;

        Transform newGround = null;
        if (Physics.SphereCast(b.center, radius, Vector3.down, out RaycastHit hit,
                               castDistance, groundLayers, QueryTriggerInteraction.Ignore))
        {
            newGround = hit.transform;
        }

        // standing on something new (or nothing): start tracking fresh
        if (newGround != ground)
        {
            ground = newGround;
            if (ground != null) groundLastPos = ground.position;
            return Vector3.zero;
        }

        if (ground == null) return Vector3.zero;

        // how far it moved since last step = its speed
        Vector3 vel = (ground.position - groundLastPos) / Time.fixedDeltaTime;
        groundLastPos = ground.position;
        return vel;
    }

    // the icy ramp calls this
    public void SetGrip(float value)
    {
        grip = value;
    }
}
}
