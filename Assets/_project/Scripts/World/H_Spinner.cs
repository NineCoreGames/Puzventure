using UnityEngine;

public class H_Spinner : MonoBehaviour
{
    // derece / saniye
    [SerializeField] private float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}