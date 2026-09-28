using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    public float hassasiyet = 0.1f;
    public float minAci = -85f;
    public float maxAci = 85f;

    private float xDonus = 0f;
    private float yDonus = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        yDonus += delta.x * hassasiyet;
        xDonus -= delta.y * hassasiyet;
        xDonus = Mathf.Clamp(xDonus, minAci, maxAci);

        transform.rotation = Quaternion.Euler(xDonus, yDonus, 0f);
    }
}