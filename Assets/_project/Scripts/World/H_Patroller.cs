using UnityEngine;

public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.0f; // metre / saniye

    private Vector3 pointA;
    private bool goingToB = true;

    void Start()
    {
        pointA = transform.position; // başladığı yer = A noktası
    }

    void Update()
    {
        // Hedef: B'ye gidiyorsak B, dönüyorsak A
        Vector3 target = goingToB ? pointB.position : pointA;

        // Hedefe doğru yumuşak hareket
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Hedefe ulaştıysak yön değiştir
        if (transform.position == target)
        {
            goingToB = !goingToB;
        }
    }
}