using UnityEngine;

namespace Puzventure.Berfin
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RuneItem : MonoBehaviour
    {
        [Header("Rune State")]
        [SerializeField] private bool isCarried = false;

        private Rigidbody runeRb;
        private Collider runeCollider;

        public bool IsCarried => isCarried;

        void Awake()
        {
            runeRb = GetComponent<Rigidbody>();
            runeCollider = GetComponent<Collider>();
        }

        /// <summary>
        /// Karakter rünü aldığında çağrılır: fizik etkilerini kapatır ve taşıyıcıya sabitler.
        /// </summary>
        public void AttachToCarrier(Transform mountPoint)
        {
            isCarried = true;
            runeRb.isKinematic = true;
            runeCollider.enabled = false;

            transform.SetParent(mountPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Rün düşürüldüğünde fizik kurallarını ve serbest hareketi geri yükler.
        /// </summary>
        public void ReleaseRune()
        {
            isCarried = false;
            transform.SetParent(null);

            runeCollider.enabled = true;
            runeRb.isKinematic = false;
            runeRb.linearVelocity = Vector3.zero;
        }
    }
}
