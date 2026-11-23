using UnityEngine;

public class N_CargoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private float delay = 4f;

    private float timer;
    private N_Cargo spawnedCargo;

    void Update()
    {
        if (spawnedCargo != null)
        {
            // Kargo hala pad üzerinde mi?
            if (spawnedCargo.transform.IsChildOf(transform))
                return;

            // Kargo oyuncu tarafından taşınıyor mu?
            if (spawnedCargo.GetComponentInParent<N_PlayerCarry>() != null)
                return;
        }

        timer -= Time.deltaTime;

        if (timer > 0f) return;

        GameObject newCargo = Instantiate(
            cargoPrefab,
            transform.position,
            transform.rotation,
            transform
        );

        spawnedCargo = newCargo.GetComponent<N_Cargo>();

        timer = delay;
    }
}