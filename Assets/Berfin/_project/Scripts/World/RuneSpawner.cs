using UnityEngine;

namespace Berfin
{
    public class RuneSpawner : MonoBehaviour //rünün yerinden tekrar oluşması...
    {
        [Header("Spawn Configuration")]
        [SerializeField] private GameObject runePrefab;
        [SerializeField] private float delay = 4f;

        private float timer;
        private GameObject currentRune;

        private void Update()
        {
            // Eğer rün referansı varsa ve sahnede hala yaşıyorsa bekle
            if (currentRune != null) return; //üst üste yeni rünlerin doğmasını engeller.

            // Süreyi geri say
            timer -= Time.deltaTime;
            if (timer > 0f) return;

            // Süre doldu: Yenisini doğur
            if (runePrefab != null)
            {
                Vector3 spawnPosition = transform.position + Vector3.up * 0.8f;
                currentRune = Instantiate(runePrefab, spawnPosition, transform.rotation);
            }

            timer = delay;
        }

      
        }
    }
//Instantiate: Verilen bir Prefab şablonunu oyun sahnesinde dinamik olarak (çalışma anında) var etmeye / üretmeye yarar.
