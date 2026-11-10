using UnityEngine;

namespace Puzventure.Rana
{
    public class RuneStone : MonoBehaviour
    {
        // Hafif mi aðýr mý? Veri dosyasý karar verir
        [SerializeField] private RuneType type;

        private Rigidbody rb;
        private BoxCollider box;
        private SphereCollider range;
        private float canPickUpAt;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            box = GetComponent<BoxCollider>();
            range = GetComponent<SphereCollider>();

            // Eðer Type yuvasý boþ unutulmuþsa Console'da kýrmýzý uyarý verir
            if (type == null)
            {
                Debug.LogError(name + ": no Rune Type assigned!");
            }
        }

        // Puan ve aðýrlýk sorularýný veri dosyasýna sorar
        public int Points()
        {
            return type != null ? type.Points() : 1;
        }

        public float Weight()
        {
            return type != null ? type.Weight() : 0f;
        }

        public bool CanBePickedUp()
        {
            return Time.time > canPickUpAt;
        }

        public void PickUp(Transform carryPoint)
        {
            if (rb != null) rb.isKinematic = true;
            if (box != null) box.enabled = false;
            if (range != null) range.enabled = false;

            transform.SetParent(carryPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Drop()
        {
            transform.SetParent(null);
            if (box != null) box.enabled = true;
            if (range != null) range.enabled = true;
            if (rb != null)
            {
                rb.isKinematic = false;
                Vector3 push = -transform.forward + Vector3.up;
                rb.AddForce(push * 3f, ForceMode.Impulse);
            }

            canPickUpAt = Time.time + 0.5f;
        }
    }
}