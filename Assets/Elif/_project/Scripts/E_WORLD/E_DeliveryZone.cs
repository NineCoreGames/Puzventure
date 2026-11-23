using UnityEngine;

public class E_DeliveryZone : MonoBehaviour
{
    // skoru artık Round tutuyor
    [SerializeField] private E_Round round;
    [SerializeField] private GameObject gate;
    [SerializeField] private int required = 3;

    private int delivered;

    void OnTriggerEnter(Collider other)
    {
        E_PlayerCarry carry = other.GetComponent<E_PlayerCarry>();
        if (carry == null) return;

        while (carry.IsCarrying())
        {
            E_Cargo cargo = carry.HandOver();
            round.AddPoints(cargo.Points());   // puanı Round'a ver
            delivered++;
            Destroy(cargo.gameObject);
        }

        if (gate != null && delivered >= required)
            gate.SetActive(false);
    }
}