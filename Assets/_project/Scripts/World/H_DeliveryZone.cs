using UnityEngine;

public class H_DeliveryZone : MonoBehaviour
{
    // Artýk skoru Round objesi tutuyor, onu buraya baðlayacaðýz[cite: 282, 283]
    [SerializeField] private H_Round round;

    void OnTriggerEnter(Collider other)
    {
        H_PlayerCarry carry = other.GetComponent<H_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        H_Cargo[] deliveredCargos = carry.HandOverAll();

        foreach (H_Cargo cargo in deliveredCargos)
        {
            // YENÝ: Kargonun puanýný Round scriptine gönderiyoruz[cite: 282, 283]
            round.AddPoints(cargo.Points());
            Destroy(cargo.gameObject);
        }

        // Debug.Log satýrýný sildik, artýk puanlarý konsolda deðil ekranda göreceðiz![cite: 282, 283]
    }
}