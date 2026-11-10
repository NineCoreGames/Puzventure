using Unity.Cinemachine;
using UnityEngine;

namespace Puzventure.Omer
{
    public class Hazard : MonoBehaviour
    {
        private Blinker blinker;
        private CinemachineImpulseSource shake; // null = no shake

        void Start()
        {
            blinker = GetComponent<Blinker>();   // yoksa null: hazard hep tehlikeli
            // the Impulse Source on this same hazard, if it has one
            shake = GetComponent<CinemachineImpulseSource>();
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
            
            // ...and give the camera a short bump
            if (shake != null) shake.GenerateImpulse();
        }
    }
}
