using UnityEngine;
using UnityEngine.UI; // UI Image bileşeni için gerekli
using TMPro;

namespace Berfin
{
    public class DeliveryZone : MonoBehaviour
    {
        [Header("Level Win Condition")]
        [Tooltip("Bölümü bitirmek için teslim edilmesi gereken rün sayısı")]
        [SerializeField] private int requiredRunes = 3;

        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image[] runeIcons; // 3 adet rün ikonunu buraya sürükleyeceğiz
        [SerializeField] private Color collectedColor = Color.yellow; // Teslim edilince olacak renk
        [SerializeField] private Color uncollectedColor = new Color(0.3f, 0.3f, 0.3f, 1f); // Başlangıçtaki sönük renk

        private int deliveredRuneCount = 0;
        private int coinScore = 0; // Sadece coin puanlarını tutar
        private bool isLevelCompleted = false;

        private void Start()
        {
            UpdateScoreUI();
            ResetRuneIcons();
        }

        public void AddBonusScore(int points)
        {
            if (isLevelCompleted) return;
            coinScore += points;
            UpdateScoreUI();
            Debug.Log($"[Rune Vault] Coin Toplandı! Toplam Coin Puanı: {coinScore}");
        }

        private void OnTriggerEnter(Collider other) //rün teslim etme anında...
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

                        // İlgili rün ikonunu sarıya boya
                        UpdateRuneIconUI();

                        Debug.Log($"[Rune Vault] Rün Teslim Edildi ({deliveredRuneCount}/{requiredRunes})");

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

        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = "Coin Score: " + coinScore;
            }
        }

        private void ResetRuneIcons()// oyun başında tüm rün ikonları sönük hale...
        {
            foreach (var icon in runeIcons)
            {
                if (icon != null) icon.color = uncollectedColor;
            }
        }

        private void UpdateRuneIconUI() //ilgili ikonun rengini parlatır
        {
            int index = deliveredRuneCount - 1;
            if (index >= 0 && index < runeIcons.Length && runeIcons[index] != null)
            {
                runeIcons[index].color = collectedColor;
            }
        }

        private void CompleteLevel()
        {
            isLevelCompleted = true;

            if (scoreText != null)
            {
                scoreText.text = "BÖLÜM BİTTİ! Toplam Coin: " + coinScore;
            }

            Debug.Log($"==========================================");
            Debug.Log($"[RUNE VAULT TAMAMLANDI!] 3 Rün Teslim Edildi! Coin Puanı: {coinScore}");
            Debug.Log($"==========================================");
        }
    }
}