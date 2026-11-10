using UnityEngine;

public class E_CargoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private float delay = 4f;

    private float timer;

    void Update()
    {
        if (GetComponentInChildren<E_Cargo>() != null) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Instantiate(cargoPrefab, transform.position,
                    transform.rotation, transform);
        timer = delay;
    }
}