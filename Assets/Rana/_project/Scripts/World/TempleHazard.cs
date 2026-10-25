using UnityEngine;

public class TempleHazard : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Gelen nesne kaþif mi ve rün taþýyor mu?
        ExplorerCarry carry = other.GetComponent<ExplorerCarry>();
        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        // Rün taþýný düþür ve konsola bilgi yaz
        carry.DropRune();
        Debug.Log(name + " run tasini dusurdu!");
    }
}