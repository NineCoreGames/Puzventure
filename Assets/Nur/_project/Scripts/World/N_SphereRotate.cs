using UnityEngine;

public class SphereRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }
}