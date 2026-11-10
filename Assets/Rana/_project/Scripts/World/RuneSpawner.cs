using UnityEngine;

namespace Puzventure.Rana
{
    public class RuneSpawner : MonoBehaviour
    {
        // Kopyalanacak rün prefab'ý ve platformun ne kadar süre boþ kalacaðý
        [SerializeField] private GameObject runePrefab;
        [SerializeField] private float delay = 4f;

        private float timer; // Yeni rün gelene kadar kalan saniye

        void Update()
        {
            // Platformun üzerinde/altýnda hala bir rün duruyor mu? Varsa hiçbir þey yapma bekle
            if (GetComponentInChildren<RuneStone>() != null) return;

            // Rün alýnmýþsa geri say: her kare geçen süreyi düþ
            timer -= Time.deltaTime;

            // Süre dolmadýysa bekle
            if (timer > 0f) return;

            // Süre doldu: Prefab'tan bu platformun üstüne bir kopya üret
            Instantiate(runePrefab, transform.position, transform.rotation, transform);

            // Bir sonraki bekleme süresini ayarla
            timer = delay;
        }
    }
}