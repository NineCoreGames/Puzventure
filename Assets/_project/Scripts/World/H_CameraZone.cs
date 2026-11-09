using UnityEngine;

public class H_CameraZone : MonoBehaviour
{
    // Bu bölgeye girildiðinde açýlacak olan Cinemachine Kamerasý
    [SerializeField] private GameObject zoneCamera;

    void OnTriggerEnter(Collider other)
    {
        // Giren kiþi "Player" etiketine sahip deðilse hiçbir þey yapma
        if (!other.CompareTag("Player")) return;

        // Player girdiyse kamerayý aktif et (Böylece Live kamera bu olur)
        zoneCamera.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Player bölgeden çýkýnca kamerayý kapat (Ana CM_Follow kamerasýna geri döner)
        zoneCamera.SetActive(false);
    }
}     
