using UnityEngine;

public class E_CameraZone : MonoBehaviour
{
    // bu bolgede acilacak kamera (Inspector'dan CM_Ramp'i surukle)
    [SerializeField] private GameObject zoneCamera;

    // Detect: bir sey bolgeye girdi
    void OnTriggerEnter(Collider other)
    {
        // Who? sadece "Player" etiketli obje
        if (!other.CompareTag("Player")) return;
        // Act: kamerayi ac, canli kamera o olur
        zoneCamera.SetActive(true);
    }

    // Detect: bir sey bolgeden cikti
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // kamerayi kapat, takip kamerasi geri gelir
        zoneCamera.SetActive(false);
    }
}
