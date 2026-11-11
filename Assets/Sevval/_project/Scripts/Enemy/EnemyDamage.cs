
using UnityEngine;

namespace Sevval
{
    public class EnemyDamage : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
                return;

            PlayerHealth health =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Düşman oyuncuya saldırdı!");
            }
        }
    }
}
