using UnityEngine;

public class Bobber : MonoBehaviour //coinlerin rünlerin yukarı aşağı süzülmesini sağlar.
{
    //serializefield inspectorda görünür diğer scriptler tarafından değiştirelemez ve görülemez.
    [SerializeField] private float bobHeight = 0.25f; // Yukarı ve aşağı kaç metre gideceği
    [SerializeField] private float bobSpeed = 2f;      // Ne kadar hızlı süzüleceği

    private Vector3 startLocalPosition;

    void Start()
    {
        // Başlangıç noktamızı hafızaya alıyoruz
        startLocalPosition = transform.localPosition;
        //position dünya uzayınaki mutlak kordinattır, localposition objenin bağlı olduğu ana nesneye göre konumunu verir.
    }

    void Update()
    {
        // Sinüs dalgası ile -bobHeight ve +bobHeight arasında yumuşak bir salınım üretiyoruz
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startLocalPosition + Vector3.up * offset;
    }
}