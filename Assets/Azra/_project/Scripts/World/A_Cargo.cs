using UnityEngine;

public class A_Cargo : MonoBehaviour
{
    // YENI (W06): hafif mi agir mi? bir veri dosyasi karar verir
    [SerializeField] private A_CargoType type;

    [SerializeField] private float dropCooldown = 0.5f;
    [SerializeField] private float dropPushUp = 2f;
    [SerializeField] private float dropPushBack = 1f;

    private Rigidbody rb;
    private Collider boxCollider;   // fiziksel govde
    private Collider pickupRange;   // alma menzili (trigger)
    private float dropTime;

    void Awake()
    {
        // Tum component'leri bir kere bul
        rb = GetComponent<Rigidbody>();
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.isTrigger) pickupRange = c;
            else boxCollider = c;
        }

        // YENI: Type bos mu? kirmizi yazsin (W03'teki sessiz bos slot gibi olmasin)
        if (type == null) Debug.LogError(name + ": no Cargo Type");
    }

    // YENI: bu tas kac puan, ne kadar agir? cevabi turu verir
    public int Points()
    {
        return type.Points();
    }

    public float Weight()
    {
        return type.Weight();
    }

    // PlayerCarry soruyor: "Bu idol alinabilir mi?"
    public bool CanBePickedUp()
    {
        return Time.time - dropTime > dropCooldown;
    }

    // Oyuncu aldiginda cagrilir
    public void PickUp(Transform carryPoint)
    {
        rb.isKinematic = true;                       // fizik durur
        boxCollider.enabled = false;                 // kafada takilmasin
        if (pickupRange != null) pickupRange.enabled = false;
        transform.SetParent(carryPoint);             // oyuncunun child'i olur
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Oyuncu dusurdugunde cagrilir
    public void Drop()
    {
        transform.SetParent(null);                   // serbest
        rb.isKinematic = false;                      // fizik tekrar
        boxCollider.enabled = true;
        if (pickupRange != null) pickupRange.enabled = true;

        // hafif yukari ve geriye itme
        Vector3 push = Vector3.up * dropPushUp + transform.forward * -dropPushBack;
        rb.linearVelocity = push;                    // Unity 6 API

        dropTime = Time.time;                        // kisa sure tekrar alinmasin
    }
}