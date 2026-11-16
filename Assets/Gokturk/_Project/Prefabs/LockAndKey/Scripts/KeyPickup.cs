using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A key floating in the world. The player walks into it to pick it up.
/// Needs a trigger collider on the same object.
/// </summary>
public class KeyPickup : MonoBehaviour
{
    [Tooltip("Must match the Key Id on the LockedDoor and the KeyIndicatorUI.")]
    public string keyId = "DoorKey";

    [Header("Idle animation")]
    public float spinSpeed = 90f;
    public float bobHeight = 0.12f;
    public float bobSpeed = 2f;

    public UnityEvent onPickedUp;

    Vector3 startPos;
    bool taken;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (taken) return;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
    }

    void OnTriggerEnter(Collider other)
    {
        if (taken || !KeyRing.IsPlayer(other)) return;

        KeyRing ring = KeyRing.Find(other, true);
        if (ring == null) return;

        taken = true;
        ring.Add(keyId);
        onPickedUp.Invoke();
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        Vector3 startScale = transform.localScale;
        Vector3 from = transform.position;
        const float duration = 0.25f;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float k = t / duration;
            transform.localScale = startScale * (1f - k);
            transform.position = from + Vector3.up * (k * 0.5f);
            transform.Rotate(Vector3.up, 900f * Time.deltaTime, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }
}
