using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();
        if (carry == null || !carry.IsCarrying()) return;

        carry.DropCargo();
        Debug.Log("Tuzak: Rün fırlatıldı!");
    }
}