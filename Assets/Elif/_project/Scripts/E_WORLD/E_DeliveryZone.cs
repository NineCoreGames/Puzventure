using UnityEngine;

public class E_DeliveryZone : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    [SerializeField] private int required = 3;

    private int delivered;
    private int score;

    void OnTriggerEnter(Collider other)
    {
        E_PlayerCarry carry = other.GetComponent<E_PlayerCarry>();
        if (carry == null) return;

        while (carry.IsCarrying())
        {
            E_Cargo cargo = carry.HandOver();

            score += cargo.Points();
            delivered++;
            Debug.Log("Delivered! +" + cargo.Points() + "   Score: " + score);

            Destroy(cargo.gameObject);
        }

        if (gate != null && delivered >= required)
            gate.SetActive(false);
    }
}