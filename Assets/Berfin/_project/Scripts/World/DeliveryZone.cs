using UnityEngine;

namespace Berfin
{
    public class DeliveryZone : MonoBehaviour
    {
        [Header("Level Win Condition")]
        [Tooltip("Bölümü bitirmek için teslim edilmesi gereken rün sayısı")]
        [SerializeField] private int requiredRunes = 3;

        private int deliveredRuneCount = 0;
        private int totalScore = 0;
        private bool isLevelCompleted = false;

        public void AddBonusScore(int points)
        {
            if (isLevelCompleted) return;
            totalScore += points;
            Debug.Log($"[Rune Vault] Güncel Toplam Skor: {totalScore}");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isLevelCompleted) return;

            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                if (carrier.HasRune)
                {
                    RuneItem deliveredRune = carrier.DeliverRune();
                    if (deliveredRune != null)
                    {
                        deliveredRuneCount++;
                        totalScore += deliveredRune.Points;

                        Debug.Log($"[Rune Vault] Rün Teslim Edildi ({deliveredRuneCount}/{requiredRunes}) | +{deliveredRune.Points} Puan | Toplam Skor: {totalScore}");

                        // Teslim edilen rünü yok et
                        Destroy(deliveredRune.gameObject);

                        // 3 rün tamamlandı mı?
                        if (deliveredRuneCount >= requiredRunes)
                        {
                            CompleteLevel();
                        }
                    }
                }
            }
        }

        private void CompleteLevel()
        {
            isLevelCompleted = true;
            Debug.Log($"==========================================");
            Debug.Log($"[RUNE VAULT TAMAMLANDI!] 3 Rün Teslim Edildi! Final Skorun: {totalScore}");
            Debug.Log($"==========================================");
        }
    }
}