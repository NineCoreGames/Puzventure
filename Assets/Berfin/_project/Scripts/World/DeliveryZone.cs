using UnityEngine;

namespace Puzventure.Berfin
{
    public class DeliveryZone : MonoBehaviour
    {
        [Header("Altar Settings")]
        [Tooltip("Teslim edilen rünlerin konulacağı veya yok edileceği alan")]
        [SerializeField] private Transform depositPoint;

        private int deliveredRuneCount = 0;

        private void OnTriggerEnter(Collider other)
        {
            // Taşıyıcı karakter teslimat alanına girdi mi?
            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                if (carrier.HasRune)
                {
                    RuneItem deliveredRune = carrier.DeliverRune();
                    if (deliveredRune != null)
                    {
                        deliveredRuneCount++;
                        Debug.Log($"[Rune Vault] Rün teslim edildi! Toplam: {deliveredRuneCount}");

                        // Rünü sunağın merkezine al veya yok et
                        if (depositPoint != null)
                        {
                            deliveredRune.transform.position = depositPoint.position;
                        }
                        else
                        {
                            deliveredRune.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
