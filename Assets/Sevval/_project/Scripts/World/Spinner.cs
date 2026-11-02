using UnityEngine;

namespace Sevval
{
public class Spinner : MonoBehaviour
{
    public Vector3 rotationSpeed;
    // degrees per SECOND
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
}
