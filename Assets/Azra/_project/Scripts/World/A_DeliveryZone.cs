using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class A_DeliveryZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Who? — giren şey oyuncu mu, elinde idol var mı?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;                  // oyuncu değil → çık
        if (!carry.IsCarrying()) return;            // elinde bir şey yok → çık

        // Act — idolu teslim al ve sahneden sil
        A_Cargo cargo = carry.HandOver();
        Destroy(cargo.gameObject);
        Debug.Log("Delivered!");
    }
}