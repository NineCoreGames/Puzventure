using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Opens/closes a double castle gate by rotating its two hinge transforms.
/// Call Open(), Close() or Toggle() from your own code, or let the player
/// press E (legacy: interactKey) while the main camera is within interactDistance.
/// </summary>
public class CastleDoor : MonoBehaviour
{
    [Header("Hinges (set by the generator)")]
    public Transform leftHinge;
    public Transform rightHinge;

    [Header("Motion")]
    [Range(0f, 120f)] public float openAngle = 95f;
    [Min(0.05f)] public float openDuration = 1.8f;
    public bool startOpen = false;

    [Header("Player interaction")]
    public bool allowKeyToggle = true;
    public KeyCode interactKey = KeyCode.E;   // used with the legacy Input Manager
    public float interactDistance = 3.5f;

    [Header("Audio (optional)")]
    public AudioClip openSound;
    public AudioClip closeSound;

    public bool IsOpen => target > 0.5f;
    public bool IsMoving => !Mathf.Approximately(t, target);

    float t;        // 0 = closed, 1 = open
    float target;
    AudioSource audioSource;

    void Start()
    {
        target = t = startOpen ? 1f : 0f;
        Apply();
        if (openSound != null || closeSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (allowKeyToggle && KeyPressed() && PlayerInRange())
            Toggle();

        if (IsMoving)
        {
            t = Mathf.MoveTowards(t, target, Time.deltaTime / openDuration);
            Apply();
        }
    }

    [ContextMenu("Open")] public void Open() => SetOpen(true);
    [ContextMenu("Close")] public void Close() => SetOpen(false);
    [ContextMenu("Toggle")] public void Toggle() => SetOpen(!IsOpen);

    public void SetOpen(bool open)
    {
        float newTarget = open ? 1f : 0f;
        if (Mathf.Approximately(newTarget, target)) return;
        target = newTarget;
        AudioClip clip = open ? openSound : closeSound;
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
    }

    void Apply()
    {
        float a = Mathf.SmoothStep(0f, 1f, t) * openAngle;
        // Both leaves swing away from the front (-Z) side, into +Z.
        if (leftHinge != null) leftHinge.localRotation = Quaternion.Euler(0f, -a, 0f);
        if (rightHinge != null) rightHinge.localRotation = Quaternion.Euler(0f, a, 0f);
    }

    bool PlayerInRange()
    {
        Camera cam = Camera.main;
        if (cam == null) return false;
        Vector3 doorCenter = transform.position + transform.up * 1.5f;
        return Vector3.Distance(cam.transform.position, doorCenter) <= interactDistance;
    }

    bool KeyPressed()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(interactKey);
#endif
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.35f);
        Gizmos.DrawWireSphere(transform.position + transform.up * 1.5f, interactDistance);
    }
}
