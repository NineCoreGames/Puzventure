using UnityEngine;
using Unity.Cinemachine;

public class A_Hazard : MonoBehaviour
{
    // Atanirsa: oyuncu odanin girisine geri gonderilir.
    // Bos birakilirsa: sadece tas dusurur.
    [SerializeField] private Transform respawnPoint;

    // YENI (W05): bu tehlikenin Impulse Source'u. null = sarsinti yok
    private CinemachineImpulseSource shake;

    void Start()
    {
        // Ayni objedeki Impulse Source'u bir kere bul
        shake = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Who? - giren oyuncu mu?
        A_PlayerCarry carry = other.GetComponent<A_PlayerCarry>();
        if (carry == null) return;

        // Act 1 - elinde tas varsa: en ustteki tas duser + kamera sarsilir
        if (carry.IsCarrying())
        {
            carry.DropCargo();
            Debug.Log(name + " knocked a rune stone off!");
            if (shake != null) shake.GenerateImpulse();
        }

        // Act 2 - eli dolu ya da bos: odanin basina don
        if (respawnPoint == null) return;
        Rigidbody rb = other.attachedRigidbody;
        rb.position = respawnPoint.position;   // isinlanma
        rb.linearVelocity = Vector3.zero;      // Unity 6 API
        Debug.Log(name + " sent the player back!");
    }
}