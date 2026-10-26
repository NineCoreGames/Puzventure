using UnityEngine;

namespace Puzventure.Azra
{
    public class A_Slippery : MonoBehaviour
    {
        // 1 = normal zemin, 0.04 = buz (neredeyse hiç tutunma yok)
        [SerializeField] private float grip = 0.04f;

        // Detect: bir þey bu yüzeye deðmeye baþladý
        void OnCollisionEnter(Collision hit)
        {
            // Who? sadece oyuncunun PlayerMove'u var
            A_PlayerMove move = hit.collider.GetComponent<A_PlayerMove>();
            // Act: oyuncunun tutunmasýný düþür
            if (move != null) move.SetGrip(grip);
        }

        // Detect: deðme bitti (oyuncu indi veya çýktý)
        void OnCollisionExit(Collision hit)
        {
            A_PlayerMove move = hit.collider.GetComponent<A_PlayerMove>();
            if (move != null) move.SetGrip(1f);   // tam tutunma geri
        }
    }
}
