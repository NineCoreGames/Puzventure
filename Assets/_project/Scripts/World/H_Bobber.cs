using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.25f; // metre yukarı-aşağı
    [SerializeField] private float bobSpeed = 2f;     // yüzme hızı

    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startLocalPosition + Vector3.up * offset;
    }
}