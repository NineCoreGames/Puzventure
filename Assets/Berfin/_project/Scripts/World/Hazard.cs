using UnityEngine;

namespace Berfin
{
    public class Hazard : MonoBehaviour
    {
        public enum HazardType { CorridorTrap, RampStep }

        [Header("Hazard Settings")]
        [SerializeField] private HazardType hazardType;
        [SerializeField] private Transform corridorSpawnPoint; // Koridorun başı
        [SerializeField] private Transform rampBottomPoint;     // Rampanın en aşağısı

        private void OnTriggerEnter(Collider other)
        {
            // Sadece Player etiketine sahip nesne tetikleyebilir
            if (!other.CompareTag("Player")) return;

            // Karakterde Character Controller varsa ışınlanırken hata vermemesi için geçici kapatıyoruz
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            // Hangi tuzak türündeyse ilgili noktaya ışınla
            if (hazardType == HazardType.CorridorTrap && corridorSpawnPoint != null)
            {
                other.transform.position = corridorSpawnPoint.position;
                Debug.Log("[Rune Vault] Koridor tuzağına basıldı! Koridorun başına ışınlandın.");
            }
            else if (hazardType == HazardType.RampStep && rampBottomPoint != null)
            {
                other.transform.position = rampBottomPoint.position;
                Debug.Log("[Rune Vault] Kırmızı basamağa basıldı! Rampanın en aşağısına atıldın.");
            }

            // Pozisyon değiştiği an Character Controller'ı tekrar aktif ediyoruz
            if (controller != null) controller.enabled = true;
        }
    }
}