using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDrop : MonoBehaviour
{
    private ExplorerCarry carry;
    private InputAction dropAction;

    void Start()
    {
        // Karakterin üstündeki mevcut taþýma script'ini alýyoruz
        carry = GetComponent<ExplorerCarry>();

        // "Interact" aksiyonu = E tuþu veya Gamepad North (Y / Üçgen) butonu
        dropAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        // E tuþuna basýldýðýnda eldeki rünü yere býrak
        if (dropAction.WasPressedThisFrame() && carry != null)
        {
            carry.DropRune();
        }
    }
}