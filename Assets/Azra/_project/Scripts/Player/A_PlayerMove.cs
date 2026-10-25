using UnityEngine;
using UnityEngine.InputSystem;

public class A_PlayerMove : MonoBehaviour
{
    // Hareketin 3 düðmesi (Inspector'dan ayarlanýr)
    [SerializeField] private float maxSpeed = 5f;      // m/s — maksimum hýz
    [SerializeField] private float acceleration = 40f; // m/s² — ivme
    [SerializeField] private float turnSpeed = 720f;   // derece/saniye — dönüþ

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 direction;    // nereye gitmek istiyoruz
    private float grip = 1f;      // 1 = normal zemin, buzda küçük olacak

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // Her kare: WASD / ok tuþlarý / sol çubuk oku
        Vector2 input = moveAction.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);
    }

    void FixedUpdate()
    {
        // Fizik adýmý: istenen hýz, mevcut hýz, yumuþak yaklaþ
        Vector3 wanted = direction * maxSpeed;
        Vector3 v = rb.linearVelocity;
        Vector3 flat = new Vector3(v.x, 0f, v.z);

        // Hýzý "wanted"a doðru, en fazla "step" kadar yaklaþtýr
        float step = acceleration * grip * Time.fixedDeltaTime;
        flat = Vector3.MoveTowards(flat, wanted, step);
        rb.linearVelocity = new Vector3(flat.x, v.y, flat.z);

        // Yönüne doðru dön
        if (direction == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(direction);
        float turn = turnSpeed * Time.fixedDeltaTime;
        look = Quaternion.RotateTowards(rb.rotation, look, turn);
        rb.MoveRotation(look);
    }

    // Buz (Slippery) bunu çaðýracak
    public void SetGrip(float value)
    {
        grip = value;
    }
}