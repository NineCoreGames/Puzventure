using System.Collections.Generic;
using UnityEngine;

public class E_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private int maxCarry = 4;
    [SerializeField] private float stackSpacing = 0.6f;

    private readonly List<E_Cargo> carried = new List<E_Cargo>();
    private Rigidbody rb;
    private float baseMass;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseMass = rb.mass;
    }

    public bool IsCarrying() => carried.Count > 0;

    public void DropCargo()
    {
        foreach (E_Cargo cargo in carried)
            cargo.Drop();
        carried.Clear();
        UpdateMass();
    }

    public E_Cargo HandOver()
    {
        if (carried.Count == 0) return null;
        E_Cargo top = carried[carried.Count - 1];
        carried.RemoveAt(carried.Count - 1);
        UpdateMass();
        return top;
    }

    void OnTriggerEnter(Collider other)
    {
        E_Cargo cargo = other.GetComponent<E_Cargo>();
        if (cargo == null || carried.Count >= maxCarry || !cargo.CanBePickedUp()) return;

        cargo.PickUp(carryPoint);
        cargo.transform.localPosition = Vector3.up * (stackSpacing * carried.Count);
        carried.Add(cargo);
        UpdateMass();
    }

    // heavier cargo, heavier courier
    void UpdateMass()
    {
        float extra = 0f;
        foreach (E_Cargo cargo in carried)
            extra += cargo.Weight();
        rb.mass = baseMass + extra;
    }
}