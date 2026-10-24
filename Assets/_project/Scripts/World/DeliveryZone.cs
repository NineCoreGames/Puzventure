using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCarry carrier = other.GetComponent<PlayerCarry>();
        if (carrier != null && carrier.IsCarrying)
        {
            Cargo delivered = carrier.HandOverCargo();
            if (delivered != null)
            {
                Destroy(delivered.gameObject);
                Debug.Log("Kargo teslim edildi!");
            }
        }
    }
}