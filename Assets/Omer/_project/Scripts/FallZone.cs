using UnityEngine;

namespace Puzventure.Omer
{
    public class FallZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            Rigidbody rb = other.attachedRigidbody;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }

            other.transform.position = new Vector3(-3f, 1.5f, 5f);
            foreach (FallingPlatform p in FindObjectsByType<FallingPlatform>())
            {
                p.ResetPlatform();
            }

            Debug.Log("Başlangıç noktasına dönüldü.");
        }
    }
}
