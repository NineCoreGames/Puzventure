using UnityEngine;

namespace Berfin
{
    public class MovingStrip : MonoBehaviour //tuzak şeritleri ileri geri hareket ettirir bu cs
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveDistance = 3f; // Ne kadar mesafe kayacak
        [SerializeField] private float speed = 2f;       // Hareket hızı
        [SerializeField] private bool moveOnX = true;    // True ise X ekseninde, False ise Z ekseninde hareket eder

        private Vector3 startPosition;

        private void Start()
        {
            // Şeritlerin ilk başladığı konumu kaydediyoruz
            startPosition = transform.position;
        }

        private void Update()
        {
            // Mathf.PingPong ile şerit sürekli ileri geri gidip gelir
            float pingPong = Mathf.PingPong(Time.time * speed, moveDistance) - (moveDistance / 2f);

            if (moveOnX)
            {
                transform.position = new Vector3(startPosition.x + pingPong, startPosition.y, startPosition.z);
            }
            else
            {
                transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z + pingPong);
            }
        }
    }
}
//Mathf.PingPong(): Belirli bir aralıkta (0 ile max arasında) sürekli gidip gelen (tenis masasındaki topun gidip gelmesi gibi) döngüsel bir değer üretmek için kullanılır