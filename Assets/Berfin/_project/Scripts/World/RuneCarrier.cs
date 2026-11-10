using UnityEngine;

namespace Berfin
{
    public class RuneCarrier : MonoBehaviour
    {
        [Header("Carry Socket")]
        [SerializeField] private Transform carryPoint;

        private RuneItem carriedRune;
        private Rigidbody rb;
        private float baseMass;

        public bool HasRune => carriedRune != null;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                baseMass = rb.mass;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HasRune) return;

            if (other.TryGetComponent<RuneItem>(out var rune))
            {
                if (!rune.CanBePickedUp()) return;

                carriedRune = rune;
                carriedRune.PickUp(carryPoint != null ? carryPoint : transform);

                // Karakteri ağırlaştır: Hafifte 1+0=1, Ağırda 1+1=2 kg kütle
                if (rb != null)
                {
                    rb.mass = baseMass + rune.Weight;
                }
            }
        }

        public void DropActiveRune()
        {
            if (!HasRune) return;

            carriedRune.Drop();
            carriedRune = null;

            // Kütleyi normale döndür
            if (rb != null) rb.mass = baseMass;
        }

        public RuneItem DeliverRune()
        {
            if (!HasRune) return null;

            RuneItem delivered = carriedRune;
            carriedRune = null;

            // Kütleyi normale döndür
            if (rb != null) rb.mass = baseMass;

            return delivered;
        }
    }
}