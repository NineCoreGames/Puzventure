using UnityEngine;

public class Spinner : MonoBehaviour //kendi ekseni etrafında dönme olayı
{
    [SerializeField] private float rotationSpeed = 90f;

    void Update()
    {
        //(0,speed,0) sadece y ekseni üzerinde dönme uygulanmış.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        //* Time.deltaTime: bilgisayar performansına bağlı olarak oyunun hızı değişmesin diye kullanılır.
    }
}