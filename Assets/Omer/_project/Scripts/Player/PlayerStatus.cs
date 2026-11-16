using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Vector3 spawnPosition;
    private KeyItem carriedKey;
    private Rigidbody rb;
    private float originalMass;
    
    [SerializeField] private float keyAddedWeight = 5f; 

    private void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalMass = rb.mass;
        }
    }

    public void PickUpKey(KeyItem key)
    {
        carriedKey = key;
        if (rb != null)
        {
            rb.mass = originalMass + keyAddedWeight;
        }
    }

    public void LoseKey()
    {
        if (carriedKey != null)
        {
            carriedKey.ResetToPad(); 
            carriedKey = null;       
            if (rb != null)
            {
                rb.mass = originalMass;
            }
        }
    }

    public bool HasKey()
    {
        return carriedKey != null;
    }

    public void UseKey()
    {
        if (carriedKey != null)
        {
            Destroy(carriedKey.gameObject); 
            carriedKey = null;       
            if (rb != null)
            {
                rb.mass = originalMass; 
            }
        }
    }

    public void RespawnPlayer()
    {
        LoseKey();
        transform.position = spawnPosition;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
    public bool IsCarried()
    {
        return carriedKey != null;
    }
}