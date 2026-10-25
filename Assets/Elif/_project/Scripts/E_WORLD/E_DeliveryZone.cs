using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();
        if (carry == null || !carry.IsCarrying()) return;

        Cargo cargo = carry.HandOver();
        Destroy(cargo.gameObject);
        Debug.Log("Rün Teslim Edildi!");
    }
}