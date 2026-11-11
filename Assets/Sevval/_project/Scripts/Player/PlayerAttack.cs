
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sevval
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject stonePrefab;
        [SerializeField] private Transform throwPoint;
        [SerializeField] private float throwForce = 12f;

        private PlayerInventory inventory;

        private void Start()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            if (Keyboard.current != null &&
                Keyboard.current.fKey.wasPressedThisFrame)
            {
                ThrowStone();
            }
        }

        private void ThrowStone()
        {
            if (inventory == null || !inventory.UseStone())
                return;

            GameObject stone = Instantiate(
                stonePrefab,
                throwPoint.position,
                throwPoint.rotation
            );

            Rigidbody rb = stone.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(
                    throwPoint.forward * throwForce,
                    ForceMode.Impulse
                );
            }
        }
    }
}
