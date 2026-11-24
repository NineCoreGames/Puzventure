using UnityEngine;

namespace Puzventure.Rana
{
    public class AltarZone : MonoBehaviour
    {
        // Skoru artýk Round objesi tutuyor
        [SerializeField] private Round round;

        void OnTriggerEnter(Collider other)
        {
            ExplorerCarry carry = other.GetComponent<ExplorerCarry>();
            if (carry == null) return;
            if (!carry.IsCarrying()) return;

            // Rünü teslim al, puanýný Round'a ekle ve objeyi yok et
            RuneStone rune = carry.HandOver();
            if (rune != null)
            {
                if (round != null)
                {
                    round.AddPoints(rune.Points());
                }
                Destroy(rune.gameObject);
            }
        }
    }
}