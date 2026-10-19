using UnityEngine;

public class H_DeliveryZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        H_PlayerCarry carry = other.GetComponent<H_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        H_Cargo cargo = carry.HandOver();
        Destroy(cargo.gameObject);

        Debug.Log("Delivered!");
    }
}

