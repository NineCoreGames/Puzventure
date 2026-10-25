using UnityEngine;

public class PulsingGoal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float pulseSpeed = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, pulseSpeed, 0f);
    }
}
