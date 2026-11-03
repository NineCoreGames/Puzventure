using UnityEngine;

public class A_Hazard : MonoBehaviour
{
    // Atanirsa: oyuncu odanin girisine geri gonderilir.
    // Bos birakilirsa: sadece tas dusurur (eski davranis).
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;

        // Act 1 - elinde tas varsa: en ustteki tas burada duser
        if (carry.IsCarrying())
        {
            carry.DropCargo();
            Debug.Log(name + " knocked a rune stone off!");
        }

        // Act 2 - eli dolu ya da bos: odanin basina don
        if (respawnPoint == null) return;
        Rigidbody rb = other.attachedRigidbody;
        rb.position = respawnPoint.position;   // isinlanma (ara kare cizilmez)
        rb.linearVelocity = Vector3.zero;      // Unity 6 API
        Debug.Log(name + " sent the player back!");
    }
}