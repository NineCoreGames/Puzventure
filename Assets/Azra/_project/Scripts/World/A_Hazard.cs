using UnityEngine;

public class A_Hazard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Who? — giren oyuncu mu, elinde idol var mı?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;                  // oyuncu değil → çık
        if (!carry.IsCarrying()) return;            // elinde bir şey yok → çık

        // Act — idolu düşür
        carry.DropCargo();
        Debug.Log(name + " knocked the cargo off!");
    }
}