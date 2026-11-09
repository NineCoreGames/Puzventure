using UnityEngine;

public class H_CargoSpawner : MonoBehaviour
{
    // Hangi prefabýn kopyalanacaðý ve pad'in kaç saniye boþ kalacaðý
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private float delay = 4f;

    private float timer; // Sonraki kargoya kalan süre

    void Update()
    {
        // Eðer bu pad'in üzerinde (çocuklarýnda) zaten bir kargo varsa hiçbir þey yapma (bekle)
        if (GetComponentInChildren<H_Cargo>() != null) return;

        // Kargo yoksa süreyi geriye doðru saydýr
        timer -= Time.deltaTime;

        // Süre sýfýrdan büyükse (daha dolmadýysa) bekle
        if (timer > 0f) return;

        // Süre doldu! Prefabýn bir kopyasýný (Clone) tam bu pad'in konumunda üret
        Instantiate(cargoPrefab, transform.position, transform.rotation, transform);

        // Bir sonraki bekleme süresini ayarla
        timer = delay;
    }
}