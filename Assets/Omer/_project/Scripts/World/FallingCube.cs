using UnityEngine;
using System.Collections; 

public class FallingCube : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f; 
    [SerializeField] private float destroyDelay = 2f; 

    private Rigidbody rb;
    private bool isFalling = false;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            
            Collider platformCollider = GetComponent<Collider>();
            
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true; 

        
        yield return new WaitForSeconds(fallDelay);

        
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        
        Destroy(gameObject, destroyDelay);
    }
}