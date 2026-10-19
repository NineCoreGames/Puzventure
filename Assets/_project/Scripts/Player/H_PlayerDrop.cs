using UnityEngine;
using UnityEngine.InputSystem;

public class H_PlayerDrop : MonoBehaviour
{
    private H_PlayerCarry carry;
    private InputAction dropAction;

    void Start()
    {
        // our own PlayerCarry, on this same object
        carry = GetComponent<H_PlayerCarry>();
        // "Interact" = the E key or the gamepad's North button
        dropAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        // one press = one drop (DropCargo checks the hands)
        if (dropAction.WasPressedThisFrame()) carry.DropCargo();
    }
}