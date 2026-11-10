using UnityEngine;

public class RotatingCargo : MonoBehaviour
{
    public float rotationSpeed = 45f;

    void Update()
    {
        transform.Rotate(
            Vector3.up * rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}
