using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Put on an object with a trigger collider in front of the door.
/// When the player enters with the right key, the chains and padlock drop and the door swings open.
/// Without the key, the padlock rattles and the UI indicator shakes.
/// </summary>
public class LockedDoor : MonoBehaviour
{
    [Tooltip("Must match the Key Id on the KeyPickup.")]
    public string keyId = "DoorKey";
    [Tooltip("Remove the key from the player when the door opens.")]
    public bool consumeKey = true;

    [Header("Door")]
    [Tooltip("The transform that rotates. Its pivot should sit on the hinge edge of the door.")]
    public Transform door;
    public Vector3 hingeAxis = Vector3.up;
    public float openAngle = 100f;
    public float openDuration = 1.2f;

    [Header("Lock visuals")]
    [Tooltip("Padlock and chains. They fall to the ground when unlocked.")]
    public Transform[] fallingParts;
    [Tooltip("Rattles when the player has no key.")]
    public Transform padlock;

    public UnityEvent onUnlocked;
    public UnityEvent onDenied;

    public bool IsOpen { get; private set; }

    Coroutine rattle;

    void OnTriggerEnter(Collider other)
    {
        if (IsOpen || !KeyRing.IsPlayer(other)) return;

        KeyRing ring = KeyRing.Find(other, false);
        if (ring != null && ring.TryUse(keyId, consumeKey))
        {
            IsOpen = true;
            StartCoroutine(Unlock());
        }
        else
        {
            KeyRing.RaiseDenied(keyId);
            onDenied.Invoke();
            if (padlock != null && rattle == null) rattle = StartCoroutine(Rattle());
        }
    }

    IEnumerator Unlock()
    {
        if (rattle != null) { StopCoroutine(rattle); rattle = null; }
        onUnlocked.Invoke();

        // Drop the padlock and chains.
        if (fallingParts != null)
        {
            foreach (Transform part in fallingParts)
            {
                if (part == null) continue;
                foreach (Collider c in part.GetComponentsInChildren<Collider>(true)) c.enabled = true;
                Rigidbody rb = part.gameObject.AddComponent<Rigidbody>();
                rb.AddForce(transform.forward * 0.6f + Random.insideUnitSphere * 0.3f, ForceMode.VelocityChange);
                rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.VelocityChange);
                Destroy(part.gameObject, 4f);
            }
        }

        yield return new WaitForSeconds(0.35f);

        // Swing the door open.
        if (door != null)
        {
            Quaternion from = door.localRotation;
            Quaternion to = from * Quaternion.AngleAxis(openAngle, hingeAxis);
            for (float t = 0f; t < openDuration; t += Time.deltaTime)
            {
                door.localRotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0f, 1f, t / openDuration));
                yield return null;
            }
            door.localRotation = to;
        }
    }

    IEnumerator Rattle()
    {
        Quaternion rest = padlock.localRotation;
        const float duration = 0.45f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float angle = Mathf.Sin(t * 45f) * 14f * (1f - t / duration);
            padlock.localRotation = rest * Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        padlock.localRotation = rest;
        rattle = null;
    }
}
