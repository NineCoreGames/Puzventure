using UnityEngine;
using Unity.Cinemachine;

public class Hazard : MonoBehaviour
{
    private CinemachineImpulseSource shake;

    void Start()
    {
        shake = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();

        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        carry.DropCargo();
        Debug.Log(name + " knocked the cargo off!");

        if (shake != null) shake.GenerateImpulse();
    }
}