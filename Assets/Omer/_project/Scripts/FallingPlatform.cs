using UnityEngine;

namespace Puzventure.Omer
{
    public class FallingPlatform : MonoBehaviour
    {
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private float fallSpeed = 5f;

        private Vector3 startPosition;
        private bool isFalling;
        private float timer;

        private void Start()
        {
            startPosition = transform.position;     // başladığı yeri hatırla
        }
        private void Update()
        {
            if (!isFalling)
                return;

            timer += Time.deltaTime;

            if (timer < delay)
                return;

            transform.position += Vector3.down
                                  * fallSpeed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isFalling = true;
            }
        }

        public void ResetPlatform()
        {
            transform.position = startPosition;
            isFalling = false;
            timer = 0f;
        }
    }
}
