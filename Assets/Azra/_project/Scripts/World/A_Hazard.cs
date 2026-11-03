using UnityEngine;

public class A_Hazard : MonoBehaviour
{
    // YENI: bos birakilirsa eskisi gibi sadece tas dusurur.
    // Atanirsa: eli bos oyuncuyu odanin girisine geri gonderir.
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;

        // Act 1 - elinde tas varsa: en ustteki tas duser
        if (carry.IsCarrying())
        {
            carry.DropCargo();
            Debug.Log(name + " knocked a rune stone off!");
            return;
        }

        // Act 2 - eli bossa: odanin basina don
        if (respawnPoint == null) return;
        Rigidbody rb = other.attachedRigidbody;
        rb.position = respawnPoint.position;   // isinlanma (ara kare cizilmez)
        rb.linearVelocity = Vector3.zero;      // Unity 6 API
        Debug.Log(name + " sent the player back!");
    }
}