using UnityEngine;

public class A_DeliveryZone : MonoBehaviour
{
    [SerializeField] private A_Round round;          // YENI (W07): skoru artik Round tutuyor
    [SerializeField] private Transform returnPoint;  // kapi kapaliysa: yeni tur icin buraya don

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu, elinde tas var mi?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        // Act - butun taslari al, puanlarini Round'a ver, sil
        while (carry.IsCarrying())
        {
            A_Cargo cargo = carry.HandOver();
            round.AddPoints(cargo.Points());
            Destroy(cargo.gameObject);   // once kullan, sonra sil
        }

        // kapi acildiysa oyuncu burada kalsin: kapidan cikabilir
        if (round.GateOpen()) return;

        // kapi hala kapali: yeni tur icin basa don
        if (returnPoint == null) return;
        Rigidbody rb = other.attachedRigidbody;
        rb.position = returnPoint.position;
        rb.linearVelocity = Vector3.zero;
    }
}