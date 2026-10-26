using UnityEngine;

namespace Puzventure.Azra
{
    public class A_Cargo : MonoBehaviour
    {
        [SerializeField] private float dropCooldown = 0.5f;
        [SerializeField] private float dropPushUp = 2f;
        [SerializeField] private float dropPushBack = 1f;

        private Rigidbody rb;
        private Collider boxCollider;
        private Collider pickupRange;
        private float dropTime;

        void Awake()
        {
            // Tüm component'leri bir kere bul, sonra hýzlý eriþ
            rb = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();

            // Sphere Collider = pickup range (trigger)
            // Ýki collider'ýmýz var: Box (fizik) + Sphere (trigger)
            // GetComponents ile hepsini al, trigger olaný bul
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.isTrigger) pickupRange = c;
                else boxCollider = c;
            }
        }

        // PlayerCarry soruyor: "Bu idol alýnabilir mi?"
        public bool CanBePickedUp()
        {
            // Az önce düþmüþse (0.5 saniye içinde), hayýr
            return Time.time - dropTime > dropCooldown;
        }

        // Oyuncu aldýðýnda çaðrýlýr
        public void PickUp(Transform carryPoint)
        {
            // Fizik durur — biz taþýyacaðýz
            rb.isKinematic = true;

            // Collider'lar kapanýr — kafada takýlmasýn, bir daha alýnmasýn
            boxCollider.enabled = false;
            if (pickupRange != null) pickupRange.enabled = false;

            // Oyuncunun child'ý olur — kafasýnda taþýnýr
            transform.SetParent(carryPoint);

            // Tam CarryPoint'in üstüne otur
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        // Oyuncu düþürdüðünde çaðrýlýr
        public void Drop()
        {
            // Parent'tan çýk — artýk oyuncunun child'ý deðil
            transform.SetParent(null);

            // Fizik tekrar açýlýr
            rb.isKinematic = false;

            // Collider'lar geri açýlýr
            boxCollider.enabled = true;
            if (pickupRange != null) pickupRange.enabled = true;

            // Hafif yukarý ve geriye itme
            Vector3 push = Vector3.up * dropPushUp + transform.forward * -dropPushBack;
            rb.linearVelocity = push;    // Unity 6 doðru API (W1 Sayfa 10)

            // Kýsa bir süre tekrar alýnmasýn
            dropTime = Time.time;
        }
    }
}
