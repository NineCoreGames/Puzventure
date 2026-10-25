using UnityEngine;

public class MovingHazard : MonoBehaviour
{
    public Transform obstacleA;
    public Transform obstacleB;

    public float speed = 2f;
    public bool moveAlongX = true;

    private Vector3 pointA;
    private Vector3 pointB;

    void Start()
    {
        if (obstacleA == null || obstacleB == null)
            return;

        Vector3 start = transform.position;

        if (moveAlongX)
        {
            pointA = new Vector3(
                obstacleA.position.x, start.y, start.z);

            pointB = new Vector3(
                obstacleB.position.x, start.y, start.z);
        }
        else
        {
            pointA = new Vector3(
                start.x, start.y, obstacleA.position.z);

            pointB = new Vector3(
                start.x, start.y, obstacleB.position.z);
        }
    }

    void Update()
    {
        if (obstacleA == null || obstacleB == null)
            return;

        float distance = Vector3.Distance(pointA, pointB);

        if (distance == 0f)
            return;

        float t = Mathf.PingPong(Time.time * speed / distance, 1f);

        transform.position = Vector3.Lerp(pointA, pointB, t);
    }
}