using UnityEngine;

namespace Berfin
{
    [RequireComponent(typeof(Rigidbody))]
    public class RuneItem : MonoBehaviour
    {
        [Header("Data Type")]
        [SerializeField] private RuneType type;

        private Rigidbody rb;
        private Collider boxCol;
        private Collider triggerCol;
        private float canPickUpAt;

        public int Points => type != null ? type.Points : 1;
        public float Weight => type != null ? type.Weight : 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            boxCol = GetComponent<BoxCollider>();
            triggerCol = GetComponent<SphereCollider>();

            if (type == null)
            {
                Debug.LogError($"{name}: Rune Type atanmamış!", this);
            }
        }

        public bool CanBePickedUp()
        {
            return Time.time > canPickUpAt;
        }

        public void PickUp(Transform carryPoint)
        {
            rb.isKinematic = true;
            if (boxCol != null) boxCol.enabled = false;
            if (triggerCol != null) triggerCol.enabled = false;

            transform.SetParent(carryPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Drop()
        {
            transform.SetParent(null);
            if (boxCol != null) boxCol.enabled = true;
            if (triggerCol != null) triggerCol.enabled = true;

            rb.isKinematic = false;
            Vector3 push = -transform.forward + Vector3.up;
            rb.AddForce(push * 3f, ForceMode.Impulse);

            canPickUpAt = Time.time + 0.5f;
        }
    }
}