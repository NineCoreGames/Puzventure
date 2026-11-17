using UnityEngine;

namespace Berfin
{
    public class RuneCarrier : MonoBehaviour //rün taşıma bırakma sırasındaki süreçler runıtem ve player arasıdna köprüdür.
    {
        [Header("Carry Socket")]
        [SerializeField] private Transform carryPoint;

        private RuneItem carriedRune;//taşınan rün nesnesinin hafızaki yeri
        private Rigidbody rb;
        private float baseMass;//ründen önceki ağırlık

        public bool HasRune => carriedRune != null;

        private void Start() //başlangıç kütlesini kaydeder.
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                baseMass = rb.mass;
            }
        }

        private void OnTriggerEnter(Collider other) //rünü yerden alma
        {
            if (HasRune) return;//tek sferde tek rün taşınır

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

        public void DropActiveRune() //rünü yere bırakma
        {
            if (!HasRune) return;

            carriedRune.Drop();
            carriedRune = null;

            // Kütleyi normale döndür
            if (rb != null) rb.mass = baseMass;
        }

        public RuneItem DeliverRune() //teslim etme...
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
//HasRune Property'si:Karakterin şu an rün taşıyıp taşımadığını dışarıdan sorgulamasını yapan sadece okunabilen...