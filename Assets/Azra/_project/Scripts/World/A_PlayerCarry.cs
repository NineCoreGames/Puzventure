using System.Collections.Generic;
using UnityEngine;

public class A_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private int maxCarry = 3;          // ayni anda en fazla kac tas
    [SerializeField] private float stackHeight = 1f;    // ust uste taslar arasi mesafe

    private List<A_Cargo> carried = new List<A_Cargo>();   // son eleman = en ustteki
    private Rigidbody rb;        // YENI (W06): oyuncunun govdesi
    private float baseMass;      // YENI: eli bosken kutlesi

    void Start()
    {
        // YENI: govdeyi bir kere bul, bos eldeki kutleyi hatirla (1)
        rb = GetComponent<Rigidbody>();
        baseMass = rb.mass;
    }

    public bool IsCarrying()
    {
        return carried.Count > 0;
    }

    public int CarryCount()
    {
        return carried.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        A_Cargo cargo = other.GetComponent<A_Cargo>();
        if (cargo == null) return;                  // tas degil
        if (carried.Count >= maxCarry) return;      // kafa dolu
        if (!cargo.CanBePickedUp()) return;         // yeni dustu

        cargo.PickUp(carryPoint);
        cargo.transform.localPosition = Vector3.up * stackHeight * carried.Count;
        carried.Add(cargo);
        UpdateMass();                               // YENI
    }

    // Tehlikeler ve E tusu: en ustteki tas duser
    public void DropCargo()
    {
        if (!IsCarrying()) return;
        int top = carried.Count - 1;
        A_Cargo cargo = carried[top];
        carried.RemoveAt(top);
        cargo.Drop();
        UpdateMass();                               // YENI
    }

    // Teslimat alani: en ustteki tasi verir
    public A_Cargo HandOver()
    {
        if (!IsCarrying()) return null;
        int top = carried.Count - 1;
        A_Cargo cargo = carried[top];
        carried.RemoveAt(top);
        UpdateMass();                               // YENI
        return cargo;
    }

    // YENI: kutle = bos kutle + kafadaki taslarin agirliklari
    void UpdateMass()
    {
        float extra = 0f;
        foreach (A_Cargo c in carried) extra += c.Weight();
        rb.mass = baseMass + extra;
    }
}