using UnityEngine;

public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f; // metre/saniye

    private Vector3 pointA;
    private bool goingToB = true;

    void Start()
    {
        // Başlangıç noktası = A (W2 Sayfa 34)
        pointA = transform.position;
    }

    void Update()
    {
        Vector3 target = goingToB ? pointB.position : pointA;

        // Time.deltaTime → frame rate'ten bağımsız hız (W2 Sayfa 40)
        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
            goingToB = !goingToB; // yön değiştir
    }
}