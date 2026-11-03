using UnityEngine;

public class A_DeliveryZone : MonoBehaviour
{
    [SerializeField] private int needed = 3;        // YENI: kapinin acilmasi icin gereken tas sayisi
    [SerializeField] private GameObject gate;       // YENI: acilacak kapi (bir sonraki level)

    private int delivered;   // simdiye kadar teslim edilen

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu, elinde tas var mi?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        // Act - kafadaki butun taslari tek tek teslim al
        while (carry.IsCarrying())
        {
            A_Cargo cargo = carry.HandOver();
            Destroy(cargo.gameObject);
            delivered++;
        }
        Debug.Log("Delivered: " + delivered + " / " + needed);

        // Hepsi geldiyse kapi acilir
        if (delivered >= needed && gate != null)
        {
            gate.SetActive(false);
            Debug.Log("The gate is open!");
        }
    }
}