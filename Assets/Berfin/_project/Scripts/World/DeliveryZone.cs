using UnityEngine;

namespace Berfin
{
    public class DeliveryZone : MonoBehaviour
    {
        [Header("Altar Settings")]
        [SerializeField] private Transform depositPoint;

        private int totalScore = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                if (carrier.HasRune)
                {
                    RuneItem deliveredRune = carrier.DeliverRune();
                    if (deliveredRune != null)
                    {
                        totalScore += deliveredRune.Points;
                        Debug.Log($"[Rune Vault] Teslim Edildi! +{deliveredRune.Points} Puan | Toplam Skor: {totalScore}");

                        // Kaidede yeniden doğacağı için teslim edilen fiziksel kopyayı yok et
                        Destroy(deliveredRune.gameObject);
                    }
                }
            }
        }
    }
}