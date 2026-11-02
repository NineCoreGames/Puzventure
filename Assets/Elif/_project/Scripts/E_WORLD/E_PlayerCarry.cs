using UnityEngine;

public class E_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    private E_Cargo carried;

    public bool IsCarrying() => carried != null;

    public void DropCargo()
    {
        if (!IsCarrying()) return;
        carried.Drop();
        carried = null;
    }

    public E_Cargo HandOver()
    {
        E_Cargo cargo = carried;
        carried = null;
        return cargo;
    }

    void OnTriggerEnter(Collider other)
    {
        E_Cargo cargo = other.GetComponent<E_Cargo>();
        if (cargo == null || IsCarrying() || !cargo.CanBePickedUp()) return;

        carried = cargo;
        cargo.PickUp(carryPoint);
    }
}