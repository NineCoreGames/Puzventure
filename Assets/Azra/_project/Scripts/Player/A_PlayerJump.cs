using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Azra
{
    public class A_PlayerJump : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 4f;

        // Hangi layer'lar "zemin" sayýlsýn? Inspector'dan "Ground" seçilecek
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
            // Space / gamepad A ? sadece basýldýðý karede
            if (!jumpAction.WasPressedThisFrame()) return;

            // Havadaysa zýplama
            if (!IsGrounded()) return;

            // Tek bir yukarý itiþ (Impulse = anlýk kuvvet)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Ayaðýmýzýn altýnda Ground var mý?
        bool IsGrounded()
        {
            // Kapsülün merkezinden aþaðý 1.1m ýþýn ? Ground'a deðiyor mu?
            return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayers);
        }
    }
}
