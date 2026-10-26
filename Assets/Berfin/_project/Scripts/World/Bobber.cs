using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.25f; // Yukarı ve aşağı kaç metre gideceği
    [SerializeField] private float bobSpeed = 2f;      // Ne kadar hızlı süzüleceği

    private Vector3 startLocalPosition;

    void Start()
    {
        // Başlangıç noktamızı hafızaya alıyoruz
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Sinüs dalgası ile -bobHeight ve +bobHeight arasında yumuşak bir salınım üretiyoruz
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startLocalPosition + Vector3.up * offset;
    }
}