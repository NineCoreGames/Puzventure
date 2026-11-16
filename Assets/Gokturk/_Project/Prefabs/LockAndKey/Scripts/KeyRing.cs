using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the keys the player is carrying. Lives on the player.
/// It is added automatically the first time an object tagged "Player" picks up a key.
/// </summary>
public class KeyRing : MonoBehaviour
{
    public static KeyRing Instance { get; private set; }

    public static event Action<string> KeyAdded;   // player picked up a key
    public static event Action<string> KeyUsed;    // key was consumed by a lock
    public static event Action<string> KeyDenied;  // player tried a lock without the key

    readonly HashSet<string> keys = new HashSet<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Instance = null;
        KeyAdded = null;
        KeyUsed = null;
        KeyDenied = null;
    }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public bool Has(string keyId)
    {
        return keys.Contains(keyId);
    }

    public void Add(string keyId)
    {
        if (keys.Add(keyId) && KeyAdded != null) KeyAdded(keyId);
    }

    /// <summary>Returns true if the player has the key. Removes it when consume is true.</summary>
    public bool TryUse(string keyId, bool consume)
    {
        if (!keys.Contains(keyId)) return false;
        if (consume)
        {
            keys.Remove(keyId);
            if (KeyUsed != null) KeyUsed(keyId);
        }
        return true;
    }

    public static void RaiseDenied(string keyId)
    {
        if (KeyDenied != null) KeyDenied(keyId);
    }

    public static bool IsPlayer(Collider other)
    {
        return other.GetComponentInParent<KeyRing>() != null
            || other.CompareTag("Player")
            || other.transform.root.CompareTag("Player");
    }

    /// <summary>Finds the key ring of whoever owns this collider.</summary>
    public static KeyRing Find(Collider other, bool createIfMissing)
    {
        KeyRing ring = other.GetComponentInParent<KeyRing>();
        if (ring == null && createIfMissing && IsPlayer(other))
            ring = other.transform.root.gameObject.AddComponent<KeyRing>();
        return ring;
    }
}
