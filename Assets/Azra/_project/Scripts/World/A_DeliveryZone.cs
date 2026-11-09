using UnityEngine;

public class A_DeliveryZone : MonoBehaviour
{
    [SerializeField] private int scoreToOpen = 7;     // kapinin acilmasi icin gereken puan
    [SerializeField] private GameObject gate;         // acilacak kapi (bir sonraki level)
    [SerializeField] private Transform returnPoint;   // puan yetmezse: yeni tur icin buraya don

    private int score;   // simdiye kadar toplanan puan (her Play'de 0)

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu, elinde tas var mi?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        // Act - kafadaki butun taslari al, puanlarini ekle, sil
        while (carry.IsCarrying())
        {
            A_Cargo cargo = carry.HandOver();
            score += cargo.Points();
            Debug.Log("Delivered! +" + cargo.Points() + "   Score: " + score + " / " + scoreToOpen);
            Destroy(cargo.gameObject);   // once kullan, sonra sil (slayt satir 18)
        }

        // Puan yetti mi? kapi acilir, oyuncu kapidan cikabilir
        if (score >= scoreToOpen)
        {
            if (gate != null && gate.activeSelf)
            {
                gate.SetActive(false);
                Debug.Log("The gate is open!");
            }
            return;
        }

        // Yetmedi: yeni tur icin basa don (A_Hazard'daki isinlanmanin aynisi)
        if (returnPoint == null) return;
        Rigidbody rb = other.attachedRigidbody;
        rb.position = returnPoint.position;
        rb.linearVelocity = Vector3.zero;
        Debug.Log("Not enough yet - next run!");
    }
}