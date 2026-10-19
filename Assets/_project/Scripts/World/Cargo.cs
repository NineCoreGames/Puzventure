using UnityEngine;
public class Cargo : MonoBehaviour
{
    private Rigidbody rb;
    private Collider box; // solid body
    private Collider range; // pickup trigger
    private float canPickUpAt; // re-pickup time
    void Awake()
    {
        // find our own components once, keep them
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        range = GetComponent<SphereCollider>();
    }
    // a question: "can I be picked up now?"
    public bool CanBePickedUp()
    {
        return Time.time >= canPickUpAt;
    }
    public void PickUp(Transform carryPoint)
    {
        rb.isKinematic = true;
        box.enabled = false;
        range.enabled = false;
        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void Drop()
    {
        transform.SetParent(null);
        box.enabled = true;
        range.enabled = true;
        rb.isKinematic = false;

        Vector3 push = -transform.forward + Vector3.up;
        rb.AddForce(push * 3f, ForceMode.Impulse);
        canPickUpAt = Time.time + 0.5f;
    }
}
