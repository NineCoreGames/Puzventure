using UnityEngine;

namespace Puzventure.Rana
{
    public class AltarZone : MonoBehaviour
    {
        private int score; // Þimdiye kadar toplanan toplam puan

        void OnTriggerEnter(Collider other)
        {
            ExplorerCarry carry = other.GetComponent<ExplorerCarry>();
            if (carry == null) return;

            // Oyuncu elinde bir rün taþýmýyorsa iþlem yapma
            if (!carry.IsCarrying()) return;

            // Rünü teslim al, puanýný ekle ve objeyi yok et
            RuneStone rune = carry.HandOver();
            if (rune != null)
            {
                score += rune.Points();
                Debug.Log("Delivered! +" + rune.Points() + " | Total Score: " + score);
                Destroy(rune.gameObject);
            }
        }
    }
}