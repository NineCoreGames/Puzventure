using UnityEngine;

namespace Puzventure.Omer
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private float distance = 3f;
        [SerializeField] private float speed = 2f;
        [SerializeField] private Vector3 moveDirection = Vector3.right; 

        private Vector3 startPosition;
        private Collider platformCollider;

        private void Start()
        {
            startPosition = transform.position;
            platformCollider = GetComponent<Collider>();
            moveDirection = moveDirection.normalized; 
        }

        private void Update()
        {
            float offset = Mathf.Sin(Time.time * speed) * distance;
            transform.position = startPosition + (moveDirection * offset);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            if (collision.collider.bounds.min.y < platformCollider.bounds.max.y - 0.1f) return;

            collision.transform.SetParent(transform);
        }
        
        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            collision.transform.SetParent(null);
        }
    }
}