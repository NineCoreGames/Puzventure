using UnityEngine;

namespace Puzventure.Elif
{
    public class PlayerCarry : MonoBehaviour
    {
        [SerializeField] private Transform carryPoint;
        private Cargo carried;

        public bool IsCarrying() => carried != null;

        public void DropCargo()
        {
            if (!IsCarrying()) return;
            carried.Drop();
            carried = null;
        }

        public Cargo HandOver()
        {
            Cargo cargo = carried;
            carried = null;
            return cargo;
        }

        void OnTriggerEnter(Collider other)
        {
            Cargo cargo = other.GetComponent<Cargo>();
            if (cargo == null || IsCarrying() || !cargo.CanBePickedUp()) return;

            carried = cargo;
            cargo.PickUp(carryPoint);
        }
    }
}
