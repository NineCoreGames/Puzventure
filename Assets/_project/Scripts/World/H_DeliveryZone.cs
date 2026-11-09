using UnityEngine;

public class H_DeliveryZone : MonoBehaviour
{
    private int score = 0; // Toplam puaný tutacaðýmýz deðiþken[cite: 232, 233]

    void OnTriggerEnter(Collider other)
    {
        H_PlayerCarry carry = other.GetComponent<H_PlayerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        H_Cargo[] deliveredCargos = carry.HandOverAll();

        foreach (H_Cargo cargo in deliveredCargos)
        {
            score += cargo.Points(); // Teslim edilen kargonun puanýný toplam skora ekle[cite: 232, 233]
            Destroy(cargo.gameObject);
        }

        // Konsola kaç kargo verdiðimizi ve güncel toplam skorumuzu yazdýralým[cite: 232, 233]
        Debug.Log(deliveredCargos.Length + " kargo teslim edildi! GUNCEL SKOR: " + score);
    }
}