using UnityEngine;
using Unity.Cinemachine; // 1. Cinemachine kütüphanesi

namespace Puzventure.Rana
{
    public class TempleHazard : MonoBehaviour
    {
        // 2. Bu tuzak üzerindeki sarsýntý kaynaðýný tutacak deðiþken
        private CinemachineImpulseSource shake;

        void Start()
        {
            // 3. Obje üzerindeki Cinemachine Impulse Source bileþenini yakalar
            shake = GetComponent<CinemachineImpulseSource>();
        }

        void OnTriggerEnter(Collider other)
        {
            // Gelen nesne kaþif mi ve rün taþýyor mu?
            ExplorerCarry carry = other.GetComponent<ExplorerCarry>();
            if (carry == null) return;
            if (!carry.IsCarrying()) return;

            // Rün taþýný düþür ve konsola bilgi yaz
            carry.DropRune();
            Debug.Log(name + " run tasini dusurdu!");

            // 4. Kameraya kýsa sarsýntý sinyalini gönder
            if (shake != null)
            {
                shake.GenerateImpulse();
            }
        }
    }
}