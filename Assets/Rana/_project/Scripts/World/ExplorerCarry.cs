using UnityEngine;

namespace Puzventure.Rana
{
    public class ExplorerCarry : MonoBehaviour
    {
        [SerializeField] private Transform carryPoint;
        private RuneStone carried;
        private Rigidbody rb;
        private float baseMass; // Karakterin yüksüz temel kütlesi

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                baseMass = rb.mass;
            }
        }

        public bool IsCarrying()
        {
            return carried != null;
        }

        void OnTriggerEnter(Collider other)
        {
            RuneStone rune = other.GetComponent<RuneStone>();
            if (rune == null) return;
            if (IsCarrying()) return;
            if (!rune.CanBePickedUp()) return;

            carried = rune;
            rune.PickUp(carryPoint);

            // Aðýr rün alýndýðýnda karaktere ekstra kütle ekle
            if (rb != null)
            {
                rb.mass = baseMass + rune.Weight();
            }
        }

        // Oyuncu rünü býraktýðýnda veya tuzaða çarptýðýnda çaðrýlýr
        public void DropRune()
        {
            if (!IsCarrying()) return;

            carried.Drop();
            carried = null;

            // Kütleyi tekrar normale döndür
            if (rb != null)
            {
                rb.mass = baseMass;
            }
        }

        // Yeþil teslimat alanýna (AltarZone) girince çaðrýlýr
        public RuneStone HandOver()
        {
            RuneStone rune = carried;
            carried = null;

            // Kütleyi tekrar normale döndür
            if (rb != null)
            {
                rb.mass = baseMass;
            }

            return rune;
        }
    }
}