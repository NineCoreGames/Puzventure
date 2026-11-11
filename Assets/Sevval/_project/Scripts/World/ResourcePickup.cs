
using UnityEngine;

namespace Sevval
{
    public class ResourcePickup : MonoBehaviour
    {
        public enum ResourceType
        {
            Wood,
            Stone,
            Potion
        }

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount = 1;

        private bool collected = false;

        private void OnTriggerEnter(Collider other)
        {
            if (collected || !other.CompareTag("Player"))
                return;

            PlayerInventory inventory =
                other.GetComponent<PlayerInventory>();

            if (inventory == null)
                return;

            switch (resourceType)
            {
                case ResourceType.Wood:
                    inventory.AddWood(amount);
                    break;

                case ResourceType.Stone:
                    inventory.AddStone(amount);
                    break;

                case ResourceType.Potion:
                    inventory.AddPotion(amount);

                    PlayerHealth health =
                        other.GetComponent<PlayerHealth>();

                    if (health != null)
                    {
                        health.Heal(25);
                    }

                    break;
            }

            collected = true;
            Destroy(gameObject);
        }
    }
}
