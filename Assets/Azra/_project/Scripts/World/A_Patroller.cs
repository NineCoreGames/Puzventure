using UnityEngine;

public class A_Patroller : MonoBehaviour
{
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;       // metre/saniye
    [SerializeField] private float waitTime = 0f;    // YENI: uclarda bekleme (kapilar icin). 0 = eskisi gibi
    [SerializeField] private float startDelay = 0f;  // YENI: ilk hareketten once bekleme (kapilar ayni anda acilmasin)

    private Vector3 pointA;
    private bool goingToB = true;
    private float waitUntil;   // bu zamana kadar bekle

    void Start()
    {
        // Baslangic noktasi = A (W2)
        pointA = transform.position;
        waitUntil = Time.time + startDelay;
    }

    void Update()
    {
        // Bekleme suresi dolmadiysa hareket etme
        if (Time.time < waitUntil) return;

        Vector3 target = goingToB ? pointB.position : pointA;

        // Time.deltaTime -> frame rate'ten bagimsiz hiz (W2)
        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            goingToB = !goingToB;                // yon degistir
            waitUntil = Time.time + waitTime;    // ucta biraz bekle
        }
    }
}