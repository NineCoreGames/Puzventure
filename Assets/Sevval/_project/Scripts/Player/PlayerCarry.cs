
using UnityEngine;
using System.Collections.Generic;

namespace Sevval
{
    public class PlayerCarry : MonoBehaviour
    {
        private PlayerInventory inventory;

        [SerializeField] private Transform carryPoint;
        [SerializeField] private float stackSpacing = 0.35f;

        private List<Cargo> carriedCargos = new List<Cargo>();

        private Rigidbody rb;
        private float baseMass;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            inventory = GetComponent<PlayerInventory>();

            if (rb != null)
            {
                baseMass = rb.mass;
            }
        }

        public bool IsCarrying()
        {
            return carriedCargos.Count > 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            Cargo cargo = other.GetComponent<Cargo>();

            if (cargo == null) return;
            if (!cargo.CanBePickedUp()) return;
            if (carriedCargos.Contains(cargo)) return;

            // Taşı listeye ekle
            carriedCargos.Add(cargo);

            // Envanterdeki taş sayısını artır
            if (inventory != null && cargo.CountsAsStone())
            {
                inventory.AddStone(1);
            }

            // Taşı karakterin üzerine yerleştir
            cargo.PickUp(carryPoint);

            UpdateStack();
            UpdateWeight();
        }

        private void UpdateStack()
        {
            for (int i = 0; i < carriedCargos.Count; i++)
            {
                carriedCargos[i].transform.localPosition =
                    Vector3.up * (i * stackSpacing);
            }
        }

        private void UpdateWeight()
        {
            if (rb == null) return;

            float totalWeight = 0f;

            foreach (Cargo cargo in carriedCargos)
            {
                totalWeight += cargo.Weight();
            }

            rb.mass = baseMass + totalWeight;
        }

        // F tuşuyla taşı fırlat
        public void DropCargo()
        {
            if (!IsCarrying()) return;

            int lastIndex = carriedCargos.Count - 1;
            Cargo cargo = carriedCargos[lastIndex];

            carriedCargos.RemoveAt(lastIndex);

            cargo.Drop();

            // Envanterdeki taş sayısını azalt
            if (inventory != null && cargo.CountsAsStone())
            {
                inventory.UseStone();
            }

            UpdateStack();
            UpdateWeight();
        }

        // Hedef bölge taşı teslim aldığında çağrılır
        public Cargo HandOver()
        {
            if (!IsCarrying()) return null;

            int lastIndex = carriedCargos.Count - 1;
            Cargo cargo = carriedCargos[lastIndex];

            carriedCargos.RemoveAt(lastIndex);

            // Teslim edilen taş envanterden de çıkar
            if (inventory != null && cargo.CountsAsStone())
            {
                inventory.UseStone();
            }

            UpdateStack();
            UpdateWeight();

            return cargo;
        }

        public int GetCargoCount()
        {
            return carriedCargos.Count;
        }
    }
}
