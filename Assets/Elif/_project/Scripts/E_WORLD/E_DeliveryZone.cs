using UnityEngine;

public class E_DeliveryZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        E_PlayerCarry carry = other.GetComponent<E_PlayerCarry>();
        if (carry == null || !carry.IsCarrying()) return;

        E_Cargo cargo = carry.HandOver();
        Destroy(cargo.gameObject);
        Debug.Log("Rün Teslim Edildi!");
    }
}