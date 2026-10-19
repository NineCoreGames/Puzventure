using UnityEngine;

public class H_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    private H_Cargo carried;

    public bool IsCarrying()
    {
        return carried != null;
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;
        carried.Drop();
        carried = null;
    }

    public H_Cargo HandOver()
    {
        H_Cargo cargo = carried;
        carried = null;
        return cargo;
    }

    void OnTriggerEnter(Collider other)
    {
        H_Cargo cargo = other.GetComponent<H_Cargo>();
        if (cargo == null) return;
        if (IsCarrying()) return;
        if (!cargo.CanBePickedUp()) return;

        carried = cargo;
        cargo.PickUp(carryPoint);
    }
}