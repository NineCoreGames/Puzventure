using UnityEngine;

public class A_Bobber : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.25f; // metre
    [SerializeField] private float bobSpeed = 2f;     // hız

    private Vector3 startLocalPosition;

    void Start()
    {
        // Başlangıç konumunu hatırla (W2 Sayfa 29)
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Sinüs dalgası → yumuşak yukarı-aşağı
        // Time.time kullanılıyor, deltaTime değil (W2 Sayfa 30)
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startLocalPosition + Vector3.up * offset;
    }
}