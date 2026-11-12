using System.Collections;
using UnityEngine;

namespace Puzventure.Rana
{
    public class RuneSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject runePrefab;
        [SerializeField] private float delay = 4f;

        [Header("Spawn Limit")]
        [Tooltip("Ýþaretliyse sonsuz rün doðurur. Ýþaretli deðilse aþaðýdaki limit geçerlidir.")]
        [SerializeField] private bool infinite = false;
        [Tooltip("Bu platformdan toplam kaç adet rün doðabileceði.")]
        [SerializeField] private int maxSpawnCount = 1;

        private int currentSpawnCount = 0;
        private GameObject currentRune;

        void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            // Ýlk taþ için bekleme veya anýnda doðma
            yield return new WaitForSeconds(0.1f);

            while (infinite || currentSpawnCount < maxSpawnCount)
            {
                if (currentRune == null)
                {
                    // Taþý platformun olduðu noktada doður
                    currentRune = Instantiate(runePrefab, transform.position, Quaternion.identity);
                    currentSpawnCount++;

                    // Limit dolduysa döngüden çýk
                    if (!infinite && currentSpawnCount >= maxSpawnCount)
                    {
                        yield break;
                    }
                }

                // Taþ oyuncu tarafýndan alýnana (yok olana) kadar bekle
                while (currentRune != null)
                {
                    yield return null;
                }

                // Taþ alýndýktan sonra yeniden doðma süresini bekle
                yield return new WaitForSeconds(delay);
            }
        }
    }
}