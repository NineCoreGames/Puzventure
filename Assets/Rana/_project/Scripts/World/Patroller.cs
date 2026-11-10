using UnityEngine;

namespace Rana
{
    public class Patroller : MonoBehaviour
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float speed = 3f;

        private Vector3 target;

        void Start()
        {
            if (pointA != null)
            {
                target = pointA.position;
            }
        }

        void Update()
        {
            if (pointA == null || pointB == null) return;

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                target = (target == pointA.position) ? pointB.position : pointA.position;
            }
        }
    }
}