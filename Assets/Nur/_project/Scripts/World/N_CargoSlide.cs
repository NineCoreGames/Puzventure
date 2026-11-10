using UnityEngine;

public class CargoSlide : MonoBehaviour
{
    public float slideSpeed = 1f;
    public float stopX = -3f;

    void Update()
    {
        if (transform.position.x <= stopX)
            return;

        transform.position += Vector3.left * slideSpeed * Time.deltaTime;
    }
}