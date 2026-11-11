
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sevval
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce = 5f;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayers;

        private Rigidbody rb;
        private InputAction jumpAction;
        private PlayerInventory inventory;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            inventory = GetComponent<PlayerInventory>();

            jumpAction = InputSystem.actions.FindAction("Jump");

            if (jumpAction == null)
            {
                Debug.LogError("Jump action bulunamadı!");
            }
        }

        void Update()
        {
            if (jumpAction == null) return;

            if (!jumpAction.WasPressedThisFrame()) return;

            if (!IsGrounded()) return;

            float weightMultiplier = 1f;

            if (inventory != null)
            {
                weightMultiplier = Mathf.Clamp(
                    1f - (inventory.TotalStoneWeight * 0.1f),
                    0.4f,
                    1f
                );
            }

            rb.AddForce(
                Vector3.up * jumpForce * weightMultiplier,
                ForceMode.Impulse
            );
        }

        bool IsGrounded()
        {
            return Physics.Raycast(
                transform.position,
                Vector3.down,
                1.1f,
                groundLayers
            );
        }
    }
}
