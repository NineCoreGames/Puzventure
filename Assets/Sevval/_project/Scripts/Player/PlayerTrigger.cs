
using UnityEngine;

namespace Sevval
{
    public class PlayerTrigger : MonoBehaviour
    {
        [SerializeField] private string message = "Collected!";
        [SerializeField] private bool removeAfter = true;
        [SerializeField] private int pointValue = 10;

        private bool collected = false;

        private static int totalScore = 0;

        void OnTriggerEnter(Collider other)
        {
            if (collected) return;
            if (!other.CompareTag("Player")) return;

            collected = true;

            totalScore += pointValue;

            ScoreManager.Instance.AddScore(pointValue);

            Debug.Log(message);

            if (removeAfter)
            {
                Destroy(gameObject);
            }
        }
    }
}
