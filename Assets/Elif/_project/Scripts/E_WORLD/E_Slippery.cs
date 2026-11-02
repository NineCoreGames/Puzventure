using UnityEngine;

public class E_Slippery : MonoBehaviour
{
    [SerializeField] private float grip = 0.04f;

    void OnCollisionEnter(Collision hit)
    {
        E_PlayerMove move = hit.collider.GetComponent<E_PlayerMove>();
        if (move != null) move.SetGrip(grip);
    }

    void OnCollisionExit(Collision hit)
    {
        E_PlayerMove move = hit.collider.GetComponent<E_PlayerMove>();
        if (move != null) move.SetGrip(1f);
    }
}