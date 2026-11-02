using UnityEngine;
using UnityEngine.InputSystem;

public class E_PlayerDrop : MonoBehaviour
{
    private E_PlayerCarry carry;
    private InputAction dropAction;

    void Start()
    {
        carry = GetComponent<E_PlayerCarry>();
        dropAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (dropAction.WasPressedThisFrame()) carry.DropCargo();
    }
}