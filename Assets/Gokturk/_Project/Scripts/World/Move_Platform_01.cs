using UnityEngine;


namespace Puzventure.Gokturk
{
    public class Move_Platform_01 : MonoBehaviour
{
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 6.0f; 
    
    private Vector3 pointA;
    private bool goingToB = true;
    private Rigidbody rb;
    private Rigidbody playerRb;
    private Vector3 platformVelocity;
    private Vector3 lastPlatformVelocity;

    void Start()
    {
        pointA = transform.position; 
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 target = goingToB ? pointB.position : pointA; 
        
        // 1. Calculate the next position
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        
        // 2. Calculate the exact physical velocity vector of the platform
        platformVelocity = (newPosition - rb.position) / Time.fixedDeltaTime;

        // 3. Move the platform physically
        rb.MovePosition(newPosition);

        // Turn around logic
        if (Vector3.Distance(rb.position, target) < 0.001f)
        {
            goingToB = !goingToB; 
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody>();
            
            if (playerRb != null)
            {
                // Calculate how much the platform's speed changed this frame (acceleration/direction switch)
                Vector3 velocityDelta = platformVelocity - lastPlatformVelocity;
                
                // Keep the player's vertical velocity intact so jumping works perfectly
                velocityDelta.y = 0; 
                
                // Add the velocity delta directly to the player so they inherit the acceleration 
                // and direction changes instantly, without locking their normal walking controls!
                playerRb.linearVelocity += velocityDelta;
            }
            
            lastPlatformVelocity = platformVelocity;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // When leaving the platform, the player naturally keeps their momentum 
            // unless your player script resets it.
            playerRb = null;
            lastPlatformVelocity = Vector3.zero;
        }
    }
}

}
