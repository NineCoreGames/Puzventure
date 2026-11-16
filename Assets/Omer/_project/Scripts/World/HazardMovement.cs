using UnityEngine;

public class HazardMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 20f; 
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(0f, 0f, -(speed * Time.deltaTime));
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}