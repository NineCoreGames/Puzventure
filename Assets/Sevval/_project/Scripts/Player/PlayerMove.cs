using UnityEngine;
using UnityEngine.InputSystem;

<<<<<<< HEAD
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float turnSpeed = 720f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 direction;
    private float grip = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Player üzerinde Rigidbody bulunamadı!");
            enabled = false;
            return;
        }

        // Kamera atanmamışsa Main Camera'yı bulmayı dene.
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("PlayerMove: Camera Transform atanmamış!");
        }

        // Move isimli Input Action'ı bul.
        moveAction = InputSystem.actions?.FindAction("Move");

        if (moveAction == null)
        {
            Debug.LogError(
                "Move action bulunamadı! Input Actions içinde action adı Move olmalı."
            );
        }
        else if (!moveAction.enabled)
        {
            moveAction.Enable();
        }
    }

    void Update()
    {
        if (moveAction == null || cameraTransform == null)
            return;

        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Hareketi yatay düzlemde tut.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        direction = forward * input.y + right * input.x;
        direction = Vector3.ClampMagnitude(direction, 1f);
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector3 velocity = rb.linearVelocity;

        Vector3 flatVelocity = new Vector3(
            velocity.x,
            0f,
            velocity.z
        );

        Vector3 targetVelocity = direction * maxSpeed;

        float step = acceleration * grip * Time.fixedDeltaTime;

        flatVelocity = Vector3.MoveTowards(
            flatVelocity,
            targetVelocity,
            step
        );

        rb.linearVelocity = new Vector3(
            flatVelocity.x,
            velocity.y,
            flatVelocity.z
        );

        // Karakter hareket ettiği yöne dönsün.
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion newRotation = Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(newRotation);
        }
    }

    // Buzlu zemin scriptinin kullanması için.
    public void SetGrip(float value)
    {
        grip = Mathf.Max(0f, value);
    }
}
=======
    public class PlayerMove : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 40f;
        [SerializeField] private float turnSpeed = 720f;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;

        private Rigidbody rb;
        private InputAction moveAction;
        private Vector3 direction;
        private float grip = 1f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError("Player üzerinde Rigidbody bulunamadı!");
                enabled = false;
                return;
            }

            // Kamera atanmamışsa Main Camera'yı bulmayı dene.
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            if (cameraTransform == null)
            {
                Debug.LogError("PlayerMove: Camera Transform atanmamış!");
            }

            // Move isimli Input Action'ı bul.
            moveAction = InputSystem.actions?.FindAction("Move");

            if (moveAction == null)
            {
                Debug.LogError(
                    "Move action bulunamadı! Input Actions içinde action adı Move olmalı."
                );
            }
            else if (!moveAction.enabled)
            {
                moveAction.Enable();
            }
        }

        void Update()
        {
            if (moveAction == null || cameraTransform == null)
                return;

            Vector2 input = moveAction.ReadValue<Vector2>();

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Hareketi yatay düzlemde tut.
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            direction = forward * input.y + right * input.x;
            direction = Vector3.ClampMagnitude(direction, 1f);
        }

        void FixedUpdate()
        {
            if (rb == null)
                return;

            Vector3 velocity = rb.linearVelocity;

            Vector3 flatVelocity = new Vector3(
                velocity.x,
                0f,
                velocity.z
            );

            Vector3 targetVelocity = direction * maxSpeed;

            float step = acceleration * grip * Time.fixedDeltaTime;

            flatVelocity = Vector3.MoveTowards(
                flatVelocity,
                targetVelocity,
                step
            );

            rb.linearVelocity = new Vector3(
                flatVelocity.x,
                velocity.y,
                flatVelocity.z
            );

            // Karakter hareket ettiği yöne dönsün.
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                Quaternion newRotation = Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    turnSpeed * Time.fixedDeltaTime
                );

                rb.MoveRotation(newRotation);
            }
        }

        // Buzlu zemin scriptinin kullanması için.
        public void SetGrip(float value)
        {
            grip = Mathf.Max(0f, value);
        }
    }
>>>>>>> c5f3652be7065b85b99ba439e34b619957a110ef
