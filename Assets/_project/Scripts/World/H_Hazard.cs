using UnityEngine;

public class H_Hazard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        H_PlayerCarry carry = other.GetComponent<H_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        carry.DropCargo();
        Debug.Log(name + " knocked the cargo off!");
    }
}

