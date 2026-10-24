using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    private Cargo currentCargo;
    private float dropCooldown = 0f;

    public bool IsCarrying => currentCargo != null;

    void Update()
    {
        if (dropCooldown > 0f)
        {
            dropCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCarrying || dropCooldown > 0f) return;

        Cargo cargo = other.GetComponent<Cargo>();
        if (cargo != null)
        {
            currentCargo = cargo;
            cargo.PickUp(carryPoint);
        }
    }

    public void DropCargo()
    {
        if (!IsCarrying) return;

        currentCargo.Drop();
        currentCargo = null;
        dropCooldown = 1.0f; // Düşürdükten sonra 1 saniye boyunca tekrar kafaya almaz!
    }

    public Cargo HandOverCargo()
    {
        if (!IsCarrying) return null;

        Cargo cargo = currentCargo;
        currentCargo = null;
        return cargo;
    }
}