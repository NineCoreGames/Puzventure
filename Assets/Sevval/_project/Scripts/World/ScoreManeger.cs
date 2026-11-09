
using UnityEngine;
using TMPro;

namespace Sevval
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        [SerializeField] private TMP_Text coinText;
        [SerializeField] private int targetScore = 100;

        private int totalScore = 0;
        private bool targetReached = false;

        private void Awake()
        {
            Instance = this;
            UpdateUI();
        }

        public void AddScore(int points)
        {
            totalScore += points;

            UpdateUI();

            Debug.Log("Total Coin: " + totalScore);

            if (totalScore >= targetScore && !targetReached)
            {
                targetReached = true;
                Debug.Log("Congratulations! 100 coins collected!");
            }
        }

        private void UpdateUI()
        {
            coinText.text = totalScore + " / " + targetScore;
        }
    }
}
