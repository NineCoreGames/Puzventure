using System.Collections.Generic;
using UnityEngine;

public class N_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private float stackSpacing = 1.1f;
    [SerializeField] private int maxCargo = 2;

    private List<N_Cargo> carried = new List<N_Cargo>();

    public bool IsCarrying()
    {
        return carried.Count > 0;
    }

    public int CargoCount()
    {
        return carried.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        TryPickUp(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryPickUp(other);
    }

    private void TryPickUp(Collider other)
    {
        if (carried.Count >= maxCargo) return;
        if (carryPoint == null) return;

        N_Cargo cargo = other.GetComponentInParent<N_Cargo>();

        if (cargo == null) return;
        if (carried.Contains(cargo)) return;
        if (!cargo.CanBePickedUp()) return;

        cargo.PickUp(carryPoint);

        cargo.transform.localPosition = new Vector3(
            0f,
            carried.Count * stackSpacing,
            0f
        );

        carried.Add(cargo);

        Debug.Log("Kargo sayisi: " + carried.Count);
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        int lastIndex = carried.Count - 1;

        N_Cargo cargo = carried[lastIndex];

        carried.RemoveAt(lastIndex);

        cargo.Drop();
    }

    public N_Cargo HandOver()
    {
        if (!IsCarrying()) return null;

        int lastIndex = carried.Count - 1;

        N_Cargo cargo = carried[lastIndex];

        carried.RemoveAt(lastIndex);

        return cargo;
    }
}