using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 2f;
    public bool moveUp = true;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float direction = moveUp ? 1f : -1f;
        float offset = Mathf.Sin(Time.time * speed)
            * distance * direction;

        transform.position = startPosition
            + Vector3.forward * offset;
    }
}
