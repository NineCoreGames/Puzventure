
using UnityEngine;

namespace Sevval
{
    public class PlayerInventory : MonoBehaviour
    {
        // Anahtar sistemi
        public bool HasKey = false;

        // Toplanabilir nesneler
        public int WoodCount = 0;
        public int StoneCount = 0;
        public int PotionCount = 0;

        // Taş ağırlığı
        [SerializeField] private float stoneWeight = 0.5f;

        public float TotalStoneWeight => StoneCount * stoneWeight;

        private PlayerMove playerMove;

        private void Start()
        {
            playerMove = GetComponent<PlayerMove>();
            UpdatePlayerWeight();
        }

        // Oyuncunun ağırlığını güncelle
        private void UpdatePlayerWeight()
        {
            if (playerMove == null) return;

            float weightEffect = 1f - (TotalStoneWeight * 0.02f);

            playerMove.SetWeight(weightEffect);
        }

        // Odun toplama
        public void AddWood(int amount)
        {
            WoodCount += amount;
            Debug.Log("Odun: " + WoodCount);
        }

        // Taş toplama
        public void AddStone(int amount)
        {
            StoneCount += amount;
            UpdatePlayerWeight();

            Debug.Log("Taş: " + StoneCount);
        }

        // Sağlık iksiri toplama
        public void AddPotion(int amount)
        {
            PotionCount += amount;
            Debug.Log("İksir: " + PotionCount);
        }

        // Taş fırlatma
        public bool UseStone()
        {
            if (StoneCount <= 0)
            {
                Debug.Log("Fırlatacak taşın yok!");
                return false;
            }

            StoneCount--;

            UpdatePlayerWeight();

            Debug.Log("Taş fırlatıldı! Kalan taş: " + StoneCount);

            return true;
        }
    }
}
