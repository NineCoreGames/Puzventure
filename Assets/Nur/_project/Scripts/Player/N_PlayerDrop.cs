using UnityEngine;
using UnityEngine.InputSystem;
using Puzventure.Gokturk;

namespace Puzventure.Nur
{
    public class PlayerDrop : MonoBehaviour
    {
        private PlayerCarry carry;
        private InputAction dropAction;

        void Start()
        {
            carry = GetComponent<PlayerCarry>();
            dropAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            if (dropAction.WasPressedThisFrame()) carry.DropCargo();
        }
    }
}
