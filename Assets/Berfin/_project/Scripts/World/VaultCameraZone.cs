using UnityEngine;

namespace Berfin
{
    [RequireComponent(typeof(BoxCollider))]
    public class VaultCameraZone : MonoBehaviour
    {
        [Header("Zone Camera Setup")]
        [Tooltip("Bu bölgeye girildiğinde devreye girecek Cinemachine kamerası")]
        [SerializeField] private GameObject zoneCamera;

        private void OnTriggerEnter(Collider other)
        {
            // Sadece ana karakter girdiğinde tetikle
            if (!other.CompareTag("Player")) return;

            if (zoneCamera != null)
            {
                zoneCamera.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (zoneCamera != null)
            {
                zoneCamera.SetActive(false);
            }
        }
    }
}