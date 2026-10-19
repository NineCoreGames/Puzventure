using UnityEngine;

public class H_Slippery : MonoBehaviour
{
    // 1 = normal ground, 0.04 = ice: hardly any grip
    [SerializeField] private float grip = 0.04f;

    // Detect: something starts touching this surface
    void OnCollisionEnter(Collision hit)
    {
        // Who? only the courier has PlayerMove
        H_PlayerMove move = hit.collider.GetComponent<H_PlayerMove>();
        // Act: the courier loses its grip
        if (move != null) move.SetGrip(grip);
    }

    // Detect: it stops touching this surface
    void OnCollisionExit(Collision hit)
    {
        H_PlayerMove move = hit.collider.GetComponent<H_PlayerMove>();
        if (move != null) move.SetGrip(1f);   // full grip again
    }
}
