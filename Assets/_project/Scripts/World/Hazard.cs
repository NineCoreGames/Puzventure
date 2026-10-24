using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCarry carrier = other.GetComponent<PlayerCarry>();
        if (carrier != null && carrier.IsCarrying)
        {
            carrier.DropCargo();
            Debug.Log("Tuzak kargoyu düşürdü!");
        }
    }
}