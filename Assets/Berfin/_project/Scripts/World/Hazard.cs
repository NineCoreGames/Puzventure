using UnityEngine;
using Unity.Cinemachine; // Cinemachine kütüphanesi

namespace Berfin
{
    [RequireComponent(typeof(Collider))]
    public class Hazard : MonoBehaviour
    {
        private CinemachineImpulseSource impulseSource;

        void Awake()
        {
            // Bu tuzaktaki titreşim kaynağını yakala
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                if (carrier.HasRune)
                {
                    // Rünü düşür
                    carrier.DropActiveRune();

                    // Kamerayı 0.2 saniye kısa ve tok bir şekilde sars
                    if (impulseSource != null)
                    {
                        impulseSource.GenerateImpulse();
                    }
                }
            }
        }
    }
}