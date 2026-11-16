using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.RespawnPlayer();
            }
        }
    }
}