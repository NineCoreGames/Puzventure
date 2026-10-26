using UnityEngine;

namespace Puzventure.Omer
{
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField] private GameObject door;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PushableBlock"))
            {
                door.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PushableBlock"))
            {
                door.SetActive(true);
            }
        }
    }
}
