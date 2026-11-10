using UnityEngine;

namespace Puzventure.Berfin
{
    public class RuneCarrier : MonoBehaviour
    {
        [Header("Socket References")]
        [Tooltip("Rünün sabitleneceği baş üstü noktası")]
        [SerializeField] private Transform carrySocket;

        private RuneItem activeRune;
        private float pickupCooldownTimer = 0f;

        public bool HasRune => activeRune != null;

        void Update()
        {
            if (pickupCooldownTimer > 0f)
            {
                pickupCooldownTimer -= Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HasRune || pickupCooldownTimer > 0f) return;

            // Çarpan nesnede rün bileşeni var mı kontrolü
            if (other.TryGetComponent<RuneItem>(out var rune))
            {
                activeRune = rune;
                rune.AttachToCarrier(carrySocket);
            }
        }

        public void DropActiveRune()
        {
            if (!HasRune) return;

            activeRune.ReleaseRune();
            activeRune = null;
            pickupCooldownTimer = 1.0f; // Düşürüldükten sonra anında geri kapmayı önleyen süre
        }

        public RuneItem DeliverRune()
        {
            if (!HasRune) return null;

            RuneItem completedRune = activeRune;
            activeRune = null;
            return completedRune;
        }
    }
}
