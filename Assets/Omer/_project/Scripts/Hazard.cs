using UnityEngine;

namespace Puzventure.Omer
{
    public class Hazard : MonoBehaviour
    {
        private Blinker blinker;

        void Start()
        {
            blinker = GetComponent<Blinker>();   // yoksa null: hazard hep tehlikeli
        }

        void OnTriggerStay(Collider other)
        {
            // Is the hazard dangerous right now?
            if (blinker != null && !blinker.IsOn) return;

            // Who? the courier, carrying cargo
            PlayerCarry carry = other.GetComponent<PlayerCarry>();
            if (carry == null) return;
            if (!carry.IsCarrying()) return;

            // Act: knock the cargo off
            carry.DropCargo();
            Debug.Log(name + " knocked the cargo off!");
        }
    }
}
