using UnityEngine;

namespace Sevval {
    public class Cargo : MonoBehaviour
    {
        // light or heavy? a data file decides
        [SerializeField] private CargoType type;
        private Rigidbody rb;
        private Collider box; // solid body
        private Collider range; // pickup trigger
        private float canPickUpAt; // re-pickup time
        void Awake()
        {
            // find our own components once, keep them
            rb = GetComponent<Rigidbody>();
            box = GetComponent<BoxCollider>();
            range = GetComponent<SphereCollider>();
            // an empty Type slot? say it in red
            if (type == null) Debug.LogError(name + ": no Cargo Type");
        }
        // two questions about this cargo, answered by its type
        public int Points()
        {
            return type.Points();
        }
        public float Weight()
        {
            return type.Weight();
        }

        public bool CountsAsStone()
        {
            return type != null && type.CountsAsStone();
        }
        // a question: "can I be picked up now?"
        public bool CanBePickedUp()
        {
            return Time.time >= canPickUpAt;
        }
        public void PickUp(Transform carryPoint)
        {
            rb.isKinematic = true; // physics stops
            box.enabled = false; // no bumping
            range.enabled = false; // no re-pickup
                                   // become a child of the carry point
            transform.SetParent(carryPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        public void Drop()
        {
            transform.SetParent(null);

            box.enabled = true;
            range.enabled = true;

            rb.isKinematic = false;

            // Oyuncunun baktığı yönü bul
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;

            Vector3 direction = player.forward + Vector3.up * 0.35f;

            direction.Normalize();

            // Taşı daha güçlü fırlat
            rb.AddForce(direction * 8f, ForceMode.Impulse);

            canPickUpAt = Time.time + 0.5f;
        }
    }
}
