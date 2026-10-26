using UnityEngine;

namespace Puzventure.Rana
{
    public class RuneStone : MonoBehaviour
    {
        private Rigidbody rb;
        private Collider box;
        private Collider range;
        private float canPickUpAt;

        void Awake()
        {
            // Kendi bilesenlerimizi hafizada tutuyoruz
            rb = GetComponent<Rigidbody>();
            box = GetComponent<BoxCollider>();
            range = GetComponent<SphereCollider>();
        }

        public bool CanBePickedUp()
        {
            return Time.time >= canPickUpAt;
        }

        public void PickUp(Transform carryPoint)
        {
            // 1. Fizik motorunu durdur (Player ile carpismamasi icin kritik)
            rb.isKinematic = true;

            // 2. Kendi carpisma kutularini kapat (Player'i firlatmasini onler)
            if (box != null) box.enabled = false;
            if (range != null) range.enabled = false;

            // 3. Kasifin basinin ustundeki tasima noktasina bagla
            transform.SetParent(carryPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Drop()
        {
            // Baglantiyi kopar
            transform.SetParent(null);

            // Carpisma kutularini ve fizigi tekrar ac
            if (box != null) box.enabled = true;
            if (range != null) range.enabled = true;
            rb.isKinematic = false;

            // Kasiften ileriye ve yukariya dogru firlat
            Vector3 push = -transform.forward + Vector3.up;
            rb.AddForce(push * 3f, ForceMode.Impulse);

            // 0.5 saniye boyunca hemen geri alinamaz
            canPickUpAt = Time.time + 0.5f;
        }
    }
}
