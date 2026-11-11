
using UnityEngine;

namespace Sevval
{
    public class BirdEnemy : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float flyingHeight = 2.5f;
        [SerializeField] private float orbitRadius = 2.5f;
        [SerializeField] private float orbitSpeed = 1.5f;
        [SerializeField] private float detectionDistance = 12f;

        private Vector3 startingPosition;
        private bool hasDetectedPlayer = false;

        [Header("Random Flight")]
        [SerializeField] private float heightChangeAmount = 1f;

        private float randomHeight;
        private float nextHeightChangeTime;
        private float orbitAngle;

        [Header("Attack")]
        [SerializeField] private float attackDistance = 0.5f;
        [SerializeField] private int damage = 10;
        
        [SerializeField] private float attackInterval = 5f;
        [SerializeField] private float diveSpeed = 8f;

        private bool isAttacking = false;
        private float nextDiveTime;
      

        [Header("Health")]
        [SerializeField] private int maxHealth = 50;
        private int currentHealth;

        private Transform player;
        private Vector3 diveTarget;
        private PlayerHealth playerHealth;

        private void Start()
        {
            currentHealth = maxHealth;

            startingPosition = transform.position;

            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
                playerHealth = playerObject.GetComponent<PlayerHealth>();
            }

            nextDiveTime = Time.time + attackInterval;
        }

        private void Update()
        {
            if (player == null)
                return;

            // Oyuncunun kuşa olan mesafesini hesapla
            float distanceToPlayer =
                Vector3.Distance(transform.position, player.position);

            // Oyuncu yaklaşınca kuş onu fark etsin
            if (!hasDetectedPlayer && distanceToPlayer <= detectionDistance)
            {
                hasDetectedPlayer = true;
                Debug.Log("Kuş oyuncuyu fark etti!");
            }

            // Kuş henüz oyuncuyu fark etmediyse kendi çevresinde uçsun
            Vector3 flightCenter = hasDetectedPlayer
                ? player.position
                : startingPosition;

            orbitAngle += orbitSpeed * Time.deltaTime;

            if (Time.time >= nextHeightChangeTime)
            {
                randomHeight = Random.Range(
                    -heightChangeAmount,
                    heightChangeAmount
                );

                nextHeightChangeTime =
                    Time.time + Random.Range(1.5f, 3f);
            }

            float currentHeight = flyingHeight + randomHeight;

            Vector3 offset = new Vector3(
                Mathf.Cos(orbitAngle) * orbitRadius,
                currentHeight,
                Mathf.Sin(orbitAngle) * orbitRadius
            );

            Vector3 targetPosition = flightCenter + offset;

            if (isAttacking)
            {
                DiveAttack();
                return;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            Vector3 movementDirection =
                targetPosition - transform.position;

            movementDirection.y = 0f;

            if (movementDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(movementDirection);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    3f * Time.deltaTime
                );
            }

            // Sadece oyuncuyu fark ettikten sonra saldır
            if (hasDetectedPlayer && Time.time >= nextDiveTime)
            {
                diveTarget = player.position + Vector3.up * 1f;
                isAttacking = true;
            }
        }

        private void DiveAttack()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                diveTarget,
                diveSpeed * Time.deltaTime
            );

            Vector3 direction = diveTarget - transform.position;

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    5f * Time.deltaTime
                );
            }

            if (Vector3.Distance(transform.position, diveTarget) < attackDistance)
            {
                Attack();

                isAttacking = false;

                nextDiveTime = Time.time + attackInterval;
            }
        }

        private void Attack()
        {
            playerHealth.TakeDamage(damage);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;

            Debug.Log("Kuşun canı: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Kuş öldü!");
            Destroy(gameObject);
        }
    }
}
