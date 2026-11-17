using UnityEngine;

namespace Berfin
{
    [RequireComponent(typeof(Collider))]
    public class VaultCoin : MonoBehaviour //altınların kendi etrafında dönmesi,süzülmesi oyunucu etkileşimi skor eklemesi...
    {
        [Header("Coin Settings")]
        [SerializeField] private int coinValue = 1;
        [SerializeField] private float rotateSpeed = 120f;

        [Header("Floating (Süzülme) Settings")]
        [SerializeField] private float floatSpeed = 5f;   // Süzülme hızı
        [SerializeField] private float floatHeight = 0.25f; // Yukarı aşağı hareket mesafesi

        private Vector3 startPosition;

        private void Start()
        {
            // Coinin ilk başladığı dünya pozisyonunu merkez alıyoruz
            startPosition = transform.position;

            GetComponent<Collider>().isTrigger = true;
        }

        private void Update()
        {
            // 1. Kendi ekseni etrafında dönme (Y ekseni)
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

            // 2. Sinüs dalgası ile havada yukarı aşağı süzülme efekti
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void OnTriggerEnter(Collider other) //coin toplanması...
        {
            if (!other.CompareTag("Player")) return;//coine çarpan nesnenin player olması lazım

            DeliveryZone altar = FindFirstObjectByType<DeliveryZone>();
            if (altar != null)
            {
                altar.AddBonusScore(coinValue);
            }

            Debug.Log($"[Rune Vault] Coin Toplandı! +{coinValue} Bonus Puan");
            Destroy(gameObject);//coin toplandıktan sonra hafızada sahnede yer kaplamasın diye direk siler.
        }
    }
}