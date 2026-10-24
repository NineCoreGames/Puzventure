using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Inspector'da ayarlanabilir hareket düğmeleri
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float turnSpeed = 720f;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 direction;   // girdinin yönü
    private float grip = 1f;     // 1 = normal zemin

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // her karede WASD veya ok tuşlarını oku
        Vector2 input = moveAction.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);
    }

    void FixedUpdate()
    {
        // gitmek istediğimiz hız ve mevcut yatay hız
        Vector3 wanted = direction * maxSpeed;
        Vector3 v = rb.linearVelocity;
        Vector3 flat = new Vector3(v.x, 0f, v.z);

        // hedeflenen hıza yumuşakça yaklaş
        float step = acceleration * grip * Time.fixedDeltaTime;
        flat = Vector3.MoveTowards(flat, wanted, step);
        rb.linearVelocity = new Vector3(flat.x, v.y, flat.z);

        // hareket yönüne doğru dön
        if (direction == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(direction);
        float turn = turnSpeed * Time.fixedDeltaTime;
        look = Quaternion.RotateTowards(rb.rotation, look, turn);
        rb.MoveRotation(look);
    }

    // buzlu rampa bu fonksiyonu çağırarak tutuşu düşürür
    public void SetGrip(float value)
    {
        grip = value;
    }
}