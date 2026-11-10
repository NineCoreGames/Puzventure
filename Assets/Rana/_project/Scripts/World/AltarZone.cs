using UnityEngine;

namespace Puzventure.Rana
{
    public class AltarZone : MonoBehaviour
    {
        private int deliveredCount = 0;

        void OnTriggerEnter(Collider other)
        {
            // Gelen nesne ExplorerCarry bilesenine sahip kasif mi?
            ExplorerCarry carry = other.GetComponent<ExplorerCarry>();
            if (carry == null) return;
            if (!carry.IsCarrying()) return;

            // Rünü kasiften al ve sahneden yok et
            RuneStone rune = carry.DeliverRune();
            Destroy(rune.gameObject);

            deliveredCount++;
            Debug.Log($"Delivered! Sunak run tasini kabul etti. ({deliveredCount}/3)");
        }
    }
}
