using UnityEngine;

public class A_CameraZone : MonoBehaviour
{
    // Bu bolgede acilacak Cinemachine Camera
    [SerializeField] private GameObject zoneCamera;

    // Detect: bir sey bolgeye girdi
    void OnTriggerEnter(Collider other)
    {
        // Who? sadece "Player" tag'li obje
        if (!other.CompareTag("Player")) return;
        // Act: bolge kamerasini ac -> en son acilan oldugu icin aktif olur
        zoneCamera.SetActive(true);
    }

    // Detect: bir sey bolgeden cikti
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // kapat -> takip kamerasi tekrar aktif olur
        zoneCamera.SetActive(false);
    }
}