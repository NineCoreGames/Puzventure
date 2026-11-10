using UnityEngine;
using UnityEngine.InputSystem;

namespace Rana
{
    public class PlayerDrop : MonoBehaviour
    {
        private ExplorerCarry carry;
        private InputAction dropAction;

        void Start()
        {
            carry = GetComponent<ExplorerCarry>();

            if (InputSystem.actions != null)
            {
                dropAction = InputSystem.actions.FindAction("Interact");
            }
        }

        void Update()
        {
            if (dropAction != null && dropAction.WasPressedThisFrame() && carry != null)
            {
                carry.DropRune();
            }
        }
    }
}