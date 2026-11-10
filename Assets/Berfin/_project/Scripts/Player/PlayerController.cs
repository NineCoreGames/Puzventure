using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Berfin
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour // İsmi PlayerController yaparak profesyonelleştirdik
    {
        [Header("Movement Dynamics")]
        [Tooltip("Maksimum yatay hareket hızı (m/s)")]
        [SerializeField] private float movementSpeed = 5.5f;

        [Tooltip("Hedef hıza ulaşma ivmesi")]
        [SerializeField] private float accelerationRate = 45f;

        [Tooltip("Dönüş yumuşaklığı ve açısal hız")]
        [SerializeField] private float rotationSpeed = 750f;

        private Rigidbody playerRb;
        private InputAction moveAction;
        private Vector3 targetMoveDirection;
        private float surfaceFrictionMultiplier = 1f; // grip yerine daha teknik bir adlandırma

        void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
            // Input Action doğrudan referanslanabilir
            moveAction = InputSystem.actions.FindAction("Move");
        }

        void Update()
        {
            // Oyuncu girdilerini okuma (WASD / Analog)
            Vector2 inputVector = moveAction.ReadValue<Vector2>();
            targetMoveDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
        }

        void FixedUpdate()
        {
            ApplyPhysicsMovement();
            ApplySteeringRotation();
        }

        private void ApplyPhysicsMovement()
        {
            Vector3 targetVelocity = targetMoveDirection * movementSpeed;
            Vector3 currentHorizontalVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

            // Yüzey tutuşuna göre ivme adımı
            float effectiveAcceleration = accelerationRate * surfaceFrictionMultiplier;

            // EĞER BUZDAYSA (surfaceFrictionMultiplier < 0.2f):
            if (surfaceFrictionMultiplier < 0.2f)
            {
                // 1. Buzda dururken veya yavaşken yukarı tırmanmayı engelle
                // Karakter yeterince hızlı değilse yukarı itiş gücünü neredeyse sıfırla
                if (currentHorizontalVel.magnitude < 2.5f)
                {
                    // Durduğu yerden tırmanmaya çalışıyorsa motor gücünü kır
                    effectiveAcceleration *= 0.15f;
                }

                // 2. Tuşa basılmıyorsa aşağı doğru kaymayı serbest bırak
                if (targetMoveDirection == Vector3.zero)
                {
                    effectiveAcceleration *= 0.05f;
                }
            }

            float velocityStep = effectiveAcceleration * Time.fixedDeltaTime;
            Vector3 adjustedHorizontalVel = Vector3.MoveTowards(currentHorizontalVel, targetVelocity, velocityStep);

            // Yerçekimi etkisini (linearVelocity.y) olduğu gibi koru
            playerRb.linearVelocity = new Vector3(adjustedHorizontalVel.x, playerRb.linearVelocity.y, adjustedHorizontalVel.z);
        }

        private void ApplySteeringRotation()
        {
            if (targetMoveDirection.sqrMagnitude < 0.001f) return;

            Quaternion targetRot = Quaternion.LookRotation(targetMoveDirection);
            float stepRot = rotationSpeed * Time.fixedDeltaTime;
            playerRb.MoveRotation(Quaternion.RotateTowards(playerRb.rotation, targetRot, stepRot));
        }

        public void ModifyFriction(float newFriction)
        {
            surfaceFrictionMultiplier = Mathf.Clamp01(newFriction);
        }
    }
}
