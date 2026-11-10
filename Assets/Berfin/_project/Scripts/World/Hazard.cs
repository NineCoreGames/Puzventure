using UnityEngine;

namespace Puzventure.Berfin
{
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<RuneCarrier>(out var carrier))
            {
                carrier.DropActiveRune();
            }
        }
    }
}
