

using UnityEngine;

namespace Sevval {
    public class KeyPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInventory inventory =
                    other.GetComponent<PlayerInventory>();

                if (inventory != null)
                {
                    inventory.HasKey = true;

                    Debug.Log("Anahtar bulundu!");
                }
            }
        }
    }
}

