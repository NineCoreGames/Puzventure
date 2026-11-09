using UnityEngine;

public class N_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;

    private N_Cargo carried;

    public bool IsCarrying()
    {
        return carried != null;
    }

    void OnTriggerEnter(Collider other)
    {
        N_Cargo cargo = other.GetComponent<N_Cargo>();

        if (cargo == null) return;
        if (IsCarrying()) return;
        if (!cargo.CanBePickedUp()) return;

        carried = cargo;
        cargo.PickUp(carryPoint);
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        carried.Drop();
        carried = null;
    }

    public N_Cargo HandOver()
    {
        N_Cargo cargo = carried;
        carried = null;
        return cargo;
    }
}