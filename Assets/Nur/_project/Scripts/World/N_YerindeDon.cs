using UnityEngine;

public class YerindeDonme : MonoBehaviour
{
    public float donmeHizi = 90f;

    void Update()
    {
        transform.Rotate(0f, donmeHizi * Time.deltaTime, 0f, Space.Self);
    }
}
