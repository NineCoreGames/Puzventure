using UnityEngine;
using Unity.Cinemachine;
public class HazardCollision : MonoBehaviour
{
    
    private CinemachineImpulseSource shake; // null = no shake

    void Start()
    {
        // the Impulse Source on this same hazard, if it has one
        shake = GetComponent<CinemachineImpulseSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null&& player.IsCarried())
            {
                // ...and give the camera a short bump
                if (shake != null) shake.GenerateImpulse();
                player.LoseKey();
            }
        }
    }
}