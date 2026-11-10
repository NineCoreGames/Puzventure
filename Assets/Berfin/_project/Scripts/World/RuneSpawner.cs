using UnityEngine;

namespace Berfin
{
    public class RuneSpawner : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField] private GameObject runePrefab;
        [SerializeField] private float delay = 4f;

        private float timer;

        private void Update()
        {
            // Kaidede hala bekleyen bir rün var mı?
            if (GetComponentInChildren<RuneItem>() != null) return;

            // Alındıysa süreyi geri say
            timer -= Time.deltaTime;
            if (timer > 0f) return;

            // Süre doldu: Yenisini doğur
            if (runePrefab != null)
            {
                Instantiate(runePrefab, transform.position, transform.rotation, transform);
            }

            timer = delay;
        }
    }
}