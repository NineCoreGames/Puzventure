using UnityEngine;
using UnityEngine.InputSystem;

namespace Berfin
{
    public class PlayerJump : MonoBehaviour //zıplama mekaniğimizi yönetir.
    {
        [SerializeField] private float jumpForce = 4f;
        // hangi katmanların zemin sayılacağı: Ground seçilmeli
        [SerializeField] private LayerMask groundLayers;

        private Rigidbody rb;
        private InputAction jumpAction; //iki satır zıplama tuşu referansıdır.

        void Start()
        {
            rb = GetComponent<Rigidbody>();//karakterin üzerindeki rigidbody fizik bileşenini hafızaya alır.
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        void Update()
        {
            // sadece Space tuşuna basıldığı karede çalışır
            if (jumpAction == null || !jumpAction.WasPressedThisFrame()) return;//waspress.. sürekli zıplamasını engeller.
            if (!IsGrounded()) return;   // havada sonsuz zıplamayı engeller

            // yukarı doğru tek vuruşluk fizik darbesi
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // karakterin yerde olup olmadığını anlamak için kullanılan yöntem
        bool IsGrounded()
        {
            // merkezimizden 1.1 metre aşağıya görünmez bir ışın gönderir
            return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayers);
        }
    }
}