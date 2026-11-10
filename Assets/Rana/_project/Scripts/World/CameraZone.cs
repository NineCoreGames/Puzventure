using UnityEngine;

namespace Puzventure.Rana
{
    public class CameraZone : MonoBehaviour
    {
        // Alana girildiðinde açýlacak olan Cinemachine kamerasý
        [SerializeField] private GameObject zoneCamera;

        void OnTriggerEnter(Collider other)
        {
            // Sadece 'Player' etiketli obje girdiðinde kamerayý aç
            if (!other.CompareTag("Player")) return;
            zoneCamera.SetActive(true);
        }

        void OnTriggerExit(Collider other)
        {
            // Oyuncu alandan çýktýðýnda kapat (CM_Follow geri devreye girer)
            if (!other.CompareTag("Player")) return;
            zoneCamera.SetActive(false);
        }
    }
}