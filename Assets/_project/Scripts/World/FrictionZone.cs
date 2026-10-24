using UnityEngine;

public class FrictionZone : MonoBehaviour
{
    [Range(0.01f, 1f)]
    [SerializeField] private float surfaceGrip = 0.05f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerController>(out var player))
        {
            player.ModifyFriction(surfaceGrip);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerController>(out var player))
        {
            player.ModifyFriction(1f);
        }
    }
}