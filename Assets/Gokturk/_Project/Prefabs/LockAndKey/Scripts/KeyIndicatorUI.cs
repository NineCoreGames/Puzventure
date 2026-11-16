using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD slot that shows whether the player is carrying the key.
/// Dim when empty, pops and glows gold on pickup, shakes red when a locked door is tried without it.
/// </summary>
public class KeyIndicatorUI : MonoBehaviour
{
    [Tooltip("Must match the Key Id on the KeyPickup.")]
    public string keyId = "DoorKey";

    public RectTransform slot;
    public Image icon;
    public Image glow;

    public Color emptyColor = new Color(1f, 1f, 1f, 0.18f);
    public Color ownedColor = new Color(1f, 0.82f, 0.25f, 1f);
    public Color deniedColor = new Color(1f, 0.25f, 0.2f, 1f);

    bool owned;
    Vector2 slotRest;
    Coroutine anim;

    void Awake()
    {
        if (slot != null) slotRest = slot.anchoredPosition;
    }

    void OnEnable()
    {
        KeyRing.KeyAdded += OnAdded;
        KeyRing.KeyUsed += OnUsed;
        KeyRing.KeyDenied += OnDenied;
    }

    void OnDisable()
    {
        KeyRing.KeyAdded -= OnAdded;
        KeyRing.KeyUsed -= OnUsed;
        KeyRing.KeyDenied -= OnDenied;
    }

    void Start()
    {
        owned = KeyRing.Instance != null && KeyRing.Instance.Has(keyId);
        ApplyRestState();
    }

    void Update()
    {
        // Soft breathing glow while the key is held.
        if (owned && anim == null && glow != null)
            SetGlow(0.35f + 0.2f * Mathf.Sin(Time.unscaledTime * 3f));
    }

    void OnAdded(string id)
    {
        if (id != keyId) return;
        owned = true;
        Play(Gain());
    }

    void OnUsed(string id)
    {
        if (id != keyId) return;
        owned = false;
        Play(Lose());
    }

    void OnDenied(string id)
    {
        if (id != keyId) return;
        Play(Deny());
    }

    void Play(IEnumerator routine)
    {
        if (anim != null) StopCoroutine(anim);
        ApplyRestState();
        anim = StartCoroutine(routine);
    }

    void ApplyRestState()
    {
        if (icon != null)
        {
            icon.color = owned ? ownedColor : emptyColor;
            icon.rectTransform.localScale = Vector3.one;
            icon.rectTransform.localRotation = Quaternion.identity;
        }
        if (slot != null) slot.anchoredPosition = slotRest;
        SetGlow(owned ? 0.35f : 0f);
    }

    void SetGlow(float alpha)
    {
        if (glow == null) return;
        Color c = ownedColor;
        c.a = alpha;
        glow.color = c;
    }

    IEnumerator Gain()
    {
        const float duration = 0.5f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = t / duration;
            float punch = Mathf.Sin(k * Mathf.PI);
            icon.color = Color.Lerp(emptyColor, ownedColor, Mathf.Clamp01(k * 3f));
            icon.rectTransform.localScale = Vector3.one * (1f + 0.5f * punch);
            icon.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -20f * punch);
            SetGlow(Mathf.Lerp(0f, 0.9f, punch) + 0.35f * k);
            yield return null;
        }
        anim = null;
        ApplyRestState();
    }

    IEnumerator Lose()
    {
        const float duration = 0.45f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = t / duration;
            icon.color = Color.Lerp(ownedColor, emptyColor, k);
            icon.rectTransform.localScale = Vector3.one * (1f - 0.3f * Mathf.Sin(k * Mathf.PI));
            SetGlow(0.35f * (1f - k));
            yield return null;
        }
        anim = null;
        ApplyRestState();
    }

    IEnumerator Deny()
    {
        const float duration = 0.45f;
        Color rest = owned ? ownedColor : emptyColor;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = t / duration;
            float fade = 1f - k;
            icon.color = Color.Lerp(deniedColor, rest, k);
            if (slot != null)
                slot.anchoredPosition = slotRest + new Vector2(Mathf.Sin(t * 60f) * 12f * fade, 0f);
            yield return null;
        }
        anim = null;
        ApplyRestState();
    }
}
