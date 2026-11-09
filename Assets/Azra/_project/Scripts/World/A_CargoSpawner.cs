using UnityEngine;

public class A_CargoSpawner : MonoBehaviour
{
    // neyi kopyalayacagiz, pad ne kadar bos kalacak
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private float delay = 4f;

    private float timer;   // bir sonraki tasa kalan saniye

    void Update()
    {
        // padin uzerinde hala bir tas bekliyor mu? o zaman bir sey yapma
        if (GetComponentInChildren<A_Cargo>() != null) return;

        // geri say: her karede biraz daha az zaman kalir
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        // sure doldu: prefabin bir kopyasi, burada, bizim child'imiz olarak
        Instantiate(cargoPrefab, transform.position,
                    transform.rotation, transform);
        timer = delay;   // bir sonraki bekleme bu kadar
    }
}