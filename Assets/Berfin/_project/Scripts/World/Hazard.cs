using UnityEngine;

namespace Berfin
{
    public class Hazard : MonoBehaviour //tuzağa bastığında rünü düşürür başlangış noktasına atar.
    {
        public enum HazardType { CorridorTrap, RampStep } // ikiihtimal tuzak var demek...

        [Header("Hazard Settings")]
        [SerializeField] private HazardType hazardType;
        [SerializeField] private Transform corridorSpawnPoint; // Koridorun başı
        [SerializeField] private Transform rampBottomPoint;     // Rampanın en aşağısı

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            // 1. Oyuncunun elinde rün taşı varsa, tuzağa değdiği yere bırak
            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                if (carrier.HasRune)
                {
                    carrier.DropActiveRune();
                    Debug.Log("[Rune Vault] Tuzağa yakalandın! Rün taşı olduğu yere düştü.");
                }
            }

            // 2. Karakter kontrolcüsünü geçici olarak kapatıp ışınlanma sırasında çakışmayı önle
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            // 3. Tuzak türüne göre ilgili noktaya ışınla
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

            // 4. Karakter kontrolcüsünü tekrar aktif et
            if (controller != null) controller.enabled = true;
        }
    }
}

//ışınlanırken character controller kapatılıp açılır!