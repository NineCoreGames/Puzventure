using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Azra
{
    public class A_PlayerDrop : MonoBehaviour
    {
        private A_PlayerCarry carry;
        private InputAction dropAction;

        void Start()
        {
            // Ayný objedeki PlayerCarry'yi bul
            carry = GetComponent<A_PlayerCarry>();
            // Interact = E tuþu veya gamepad'in North butonu
            dropAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            // E basýldýysa ? PlayerCarry'ye düþür de
            if (dropAction.WasPressedThisFrame()) carry.DropCargo();
        }
    }
}
