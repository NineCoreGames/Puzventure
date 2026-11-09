using UnityEngine;
using System.Collections.Generic;

public class H_PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;

    private List<H_Cargo> carriedCargos = new List<H_Cargo>();

    // Hocanýn W06'da gösterdiði Rigidbody ve temel kütle deðiþkenleri[cite: 234]
    private Rigidbody rb;
    private float baseMass;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseMass = rb.mass; // Karakterin baþlangýç aðýrlýðýný hafýzaya alýr (Genelde 1'dir)[cite: 234, 235]
    }

    public bool IsCarrying()
    {
        return carriedCargos.Count > 0;
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        foreach (H_Cargo c in carriedCargos)
        {
            c.Drop();
        }
        carriedCargos.Clear();

        UpdateMass(); // Kargolar düþtüðü için aðýrlýðýmýzý eski haline döndürür
    }

    public H_Cargo[] HandOverAll()
    {
        H_Cargo[] cargosToDeliver = carriedCargos.ToArray();
        carriedCargos.Clear();

        UpdateMass(); // Kargolar teslim edildiði için aðýrlýðýmýzý eski haline döndürür

        return cargosToDeliver;
    }

    void OnTriggerEnter(Collider other)
    {
        H_Cargo cargo = other.GetComponent<H_Cargo>();
        if (cargo == null) return;

        if (!cargo.CanBePickedUp()) return;

        float yOffset = carriedCargos.Count * 1.0f;

        carriedCargos.Add(cargo);
        cargo.PickUp(carryPoint);
        cargo.transform.localPosition = new Vector3(0, yOffset, 0);

        UpdateMass(); // Yeni kargo alýndýðýnda aðýrlýðýmýzý artýrýr
    }

    // Bizim sistemimize özel: Listedeki tüm kargolarýn aðýrlýðýný toplayýp karaktere yükler
    private void UpdateMass()
    {
        float totalExtraWeight = 0f;
        foreach (H_Cargo c in carriedCargos)
        {
            totalExtraWeight += c.Weight(); // CT_Heavy ise 1, CT_Light ise 0 ekler[cite: 221, 235]
        }
        rb.mass = baseMass + totalExtraWeight; // Zýplama gücümüz bu aðýrlýða göre düþer
    }
}