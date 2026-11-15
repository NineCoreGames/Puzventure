
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sevval
{
    public class PlayerAttack : MonoBehaviour
    {
        private PlayerCarry playerCarry;

        private void Start()
        {
            playerCarry = GetComponent<PlayerCarry>();
        }

        private void Update()
        {
            if (Keyboard.current != null &&
                Keyboard.current.fKey.wasPressedThisFrame)
            {
                ThrowStone();
            }
        }

        private void ThrowStone()
        {
            if (playerCarry == null) return;
            if (!playerCarry.IsCarrying()) return;

            playerCarry.DropCargo();
        }
    }
}
