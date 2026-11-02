
using UnityEngine;

namespace Sevval
{

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 2.5f, -4f);
    public float positionSmooth = 10f;
    public float rotationSmooth = 10f;

    private float fixedYRotation;

    void Start()
    {
        if (player != null)
        {
            fixedYRotation = player.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Kamera oyuncunun konumunu takip eder,
        // ancak oyuncunun dönüşünü takip etmez.
        Quaternion cameraRotation =
            Quaternion.Euler(0f, fixedYRotation, 0f);

        Vector3 targetPosition =
            player.position + cameraRotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            positionSmooth * Time.deltaTime
        );

        // Kamera oyuncuya bakar.
        Vector3 lookTarget =
            player.position + Vector3.up * 1.2f;

        Quaternion targetRotation = Quaternion.LookRotation(
            lookTarget - transform.position
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSmooth * Time.deltaTime
        );
    }
}

}
