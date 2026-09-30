using UnityEngine;

public class RuneStone : MonoBehaviour
{
    [Header("Rün Bilgileri")]
    public int runeID = 1;
    public bool isCarried = false;
    public bool isDelivered = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !isCarried)
        {
            Debug.Log($"Rün #{runeID} alýndý!");
        }
    }
}