using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Berfin
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 4f;
        // hangi katmanların zemin sayılacağı: Ground seçilmeli
        [SerializeField] private LayerMask groundLayers;

        private Rigidbody rb;
        private InputAction jumpAction;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        void Update()
        {
            // sadece Space tuşuna basıldığı karede çalışır
            if (!jumpAction.WasPressedThisFrame()) return;
            if (!IsGrounded()) return;   // havada zıplamayı engeller

            // yukarı doğru tek vuruşluk fizik darbesi
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // ayakların altında Ground katmanında bir şey var mı?
        bool IsGrounded()
        {
            // merkezimizden 1.1 metre aşağıya görünmez bir ışın gönderir
            return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayers);
        }
    }
}
