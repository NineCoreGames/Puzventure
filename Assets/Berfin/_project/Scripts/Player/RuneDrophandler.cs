using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzventure.Berfin
{
    [RequireComponent(typeof(RuneCarrier))]
    public class PlayerDrop : MonoBehaviour
    {
        private RuneCarrier carrier;
        private InputAction dropAction;

        void Awake()
        {
            carrier = GetComponent<RuneCarrier>();
            dropAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            // E tuşuna (veya Gamepad Interact) basıldığında rünü bırak
            if (dropAction != null && dropAction.WasPressedThisFrame())
            {
                carrier.DropActiveRune();
            }
        }
    }
}
