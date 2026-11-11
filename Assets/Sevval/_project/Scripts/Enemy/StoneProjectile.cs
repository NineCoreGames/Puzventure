
using UnityEngine;

namespace Sevval
{
    public class StoneProjectile : MonoBehaviour
    {
        [SerializeField] private int damage = 25;

        private void OnCollisionEnter(Collision collision)
        {
            BirdEnemy bird =
                collision.gameObject.GetComponentInParent<BirdEnemy>();

            if (bird != null)
            {
                bird.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
