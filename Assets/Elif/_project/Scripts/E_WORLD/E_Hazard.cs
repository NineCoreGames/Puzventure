using UnityEngine;
using Unity.Cinemachine;

public class E_Hazard : MonoBehaviour
{
    private CinemachineImpulseSource shake;   // null = sarsinti yok

    void Start()
    {
        // ayni objedeki Impulse Source'u bir kere bul
        shake = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        E_PlayerCarry carry = other.GetComponent<E_PlayerCarry>();
        if (carry == null || !carry.IsCarrying()) return;

        carry.DropCargo();
        Debug.Log("Tuzak: Rün fırlatıldı!");

        // kameraya kisa bir sarsinti gonder
        if (shake != null) shake.GenerateImpulse();
    }
}
