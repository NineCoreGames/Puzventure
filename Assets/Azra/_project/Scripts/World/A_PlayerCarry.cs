using System.Collections.Generic;
using UnityEngine;

public class A_PlayerCarry : MonoBehaviour
{
    // Inspector'da atanacak: oyuncunun kafasindaki tasima noktasi
    [SerializeField] private Transform carryPoint;
    [SerializeField] private int maxCarry = 3;          // YENI: ayni anda en fazla kac run tasi
    [SerializeField] private float stackHeight = 1f;    // YENI: ust uste taslar arasi mesafe

    // YENI: tek idol yerine bir liste. Son eleman = en ustteki tas
    private List<A_Cargo> carried = new List<A_Cargo>();

    // Baska script'ler "elinde bir sey var mi?" diye soracak
    public bool IsCarrying()
    {
        return carried.Count > 0;
    }

    // Kac tas tasiyoruz?
    public int CarryCount()
    {
        return carried.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        // Who? - dokundugum sey idol mu?
        A_Cargo cargo = other.GetComponent<A_Cargo>();
        if (cargo == null) return;

        // Kafam dolu mu?
        if (carried.Count >= maxCarry) return;

        // Yeni dusmus olabilir
        if (!cargo.CanBePickedUp()) return;

        // Act - al, kafadaki yiginin en ustune koy
        cargo.PickUp(carryPoint);
        cargo.transform.localPosition = Vector3.up * stackHeight * carried.Count;
        carried.Add(cargo);
    }

    // Tehlikeler ve E tusu bunu cagirir: sadece EN USTTEKI tas duser
    public void DropCargo()
    {
        if (!IsCarrying()) return;
        int top = carried.Count - 1;
        A_Cargo cargo = carried[top];
        carried.RemoveAt(top);
        cargo.Drop();
    }

    // Teslimat alani bunu cagirir: en ustteki tasi verir
    public A_Cargo HandOver()
    {
        if (!IsCarrying()) return null;
        int top = carried.Count - 1;
        A_Cargo cargo = carried[top];
        carried.RemoveAt(top);
        return cargo;
    }
}