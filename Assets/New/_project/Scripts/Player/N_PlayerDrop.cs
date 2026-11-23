using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Nur
{
    public class PlayerDrop : MonoBehaviour
    {
        private N_PlayerCarry carry;
        private InputAction dropAction;

        void Start()
        {
            carry = GetComponent<N_PlayerCarry>();
            dropAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            if (dropAction.WasPressedThisFrame())
                carry.DropCargo();
        }
    }
}