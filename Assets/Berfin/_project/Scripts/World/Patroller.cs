using UnityEngine;

public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.0f; // Saniyedeki hızı (metre/sn)

    private Vector3 pointA;
    private bool goingToB = true;

    void Start()
    {
        // Başladığımız konumu A noktası olarak hafızaya alıyoruz
        pointA = transform.position;
    }

    void Update()
    {
        // Hedef B mi yoksa A mı?
        Vector3 target = goingToB ? pointB.position : pointA;

        // Hedefe doğru Time.deltaTime ile FPS'ten bağımsız şekilde ilerle
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Hedefe vardıysa yön değiştir
        if (transform.position == target)
        {
            goingToB = !goingToB;
        }
    }
}