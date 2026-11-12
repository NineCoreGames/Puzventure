using UnityEngine;

namespace Berfin
{
    [RequireComponent(typeof(Collider))]
    public class VaultCoin : MonoBehaviour
    {
        [Header("Coin Settings")]
        [SerializeField] private int coinValue = 1;
        [SerializeField] private float rotateSpeed = 120f;

        private void Awake()
        {
            // Coin içinden geçilebilir bir tetikleyici olmalı
            GetComponent<Collider>().isTrigger = true;
        }

        private void Update()
        {
            // Kendi ekseninde dönerek dikkat çeksin
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Sadece Player dokunduğunda toplansın
            if (!other.CompareTag("Player")) return;

            // Sunaktaki skor yöneticisine veya doğrudan DeliveryZone'a puan ekle
            DeliveryZone altar = FindFirstObjectByType<DeliveryZone>();
            if (altar != null)
            {
                altar.AddBonusScore(coinValue);
            }

            Debug.Log($"[Rune Vault] Coin Toplandı! +{coinValue} Bonus Puan");
            Destroy(gameObject);
        }
    }
}