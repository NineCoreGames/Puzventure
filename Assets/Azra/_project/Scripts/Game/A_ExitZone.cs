using UnityEngine;

public class A_ExitZone : MonoBehaviour
{
    [SerializeField] private A_Round round;

    // Detect: kapinin arkasindaki bolgeye bir sey girdi
    void OnTriggerEnter(Collider other)
    {
        // Who? sadece oyuncu (A_CameraZone gibi tag ile)
        if (!other.CompareTag("Player")) return;
        // kapi acik degilse cikis yok (guvenlik icin)
        if (!round.GateOpen()) return;
        // Act: turu "kacti" olarak bitir
        round.Escape();
    }
}