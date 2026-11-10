using UnityEngine;

namespace Puzventure.Omer
{
    public class FinishZone : MonoBehaviour
    {
        private bool levelCompleted;

        private void OnTriggerEnter(Collider other)
        {
            if (levelCompleted)
                return;

            if (!other.CompareTag("Player"))
                return;

            levelCompleted = true;

            Debug.Log("Tebrikler! Parkuru tamamladın.");
        }
    }
}
