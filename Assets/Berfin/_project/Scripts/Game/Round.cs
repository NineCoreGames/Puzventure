using UnityEngine;
using TMPro;

namespace Berfin
{
    public class Round : MonoBehaviour
    {
        [Header("Round & HUD")]
        [SerializeField] private float roundLength = 90f;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private GameOver gameOver;

        private int score;
        private float timeLeft;
        private bool over;

        private void Start()
        {
            timeLeft = roundLength;
        }

        private void Update()
        {
            if (over) return;

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;

            ShowHud();

            if (timeLeft <= 0f)
            {
                EndRound("Time's up!");
            }
        }

        public void AddPoints(int points)
        {
            if (over) return;
            score += points;
        }

        public void EndRound(string message)
        {
            if (over) return;
            over = true;
            Time.timeScale = 0f;
            gameOver.Show(message, score);
        }

        private void ShowHud()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score " + score;
            }

            int seconds = Mathf.CeilToInt(timeLeft);
            int minutes = seconds / 60;
            if (timeText != null)
            {
                timeText.text = minutes + ":" + (seconds % 60).ToString("00");
            }
        }
    }
}