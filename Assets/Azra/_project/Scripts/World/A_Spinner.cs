using UnityEngine;

public class A_Spinner : MonoBehaviour
{
    // Inspector'da görünür ama başka script değiştiremez (W2 Sayfa 20)
    [SerializeField] private float rotationSpeed = 60f; // derece/saniye

    void Update()
    {
        // Time.deltaTime → frame rate'ten bağımsız dönüş (W2 Sayfa 24)
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}