using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;

        transform.position = startPosition
                             + Vector3.right * offset;
    }
}