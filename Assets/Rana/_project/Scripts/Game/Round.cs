using TMPro;
using UnityEngine;

namespace Puzventure.Rana
{
    public class Round : MonoBehaviour
    {
        // Raund süresi, HUD metinleri ve bitiþ ekraný
        [SerializeField] private float roundLength = 90f;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private GameOver gameOver;

        private int score;       // Bu raunddaki puan
        private float timeLeft;  // Bitiþe kalan saniye
        private bool over;       // Raund bitti mi?

        void Start()
        {
            timeLeft = roundLength;
            ShowHud();
        }

        void Update()
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

        // Yeþil teslimat alaný (AltarZone) puan eklemek için bunu çaðýrýr
        public void AddPoints(int points)
        {
            if (over) return;
            score += points;
            ShowHud();
        }

        // Süre bitince veya baþka bir tetikleyici raundu sonlandýrýnca çalýþýr
        public void EndRound(string message)
        {
            if (over) return;

            over = true;
            Time.timeScale = 0f;

            if (gameOver != null)
            {
                gameOver.Show(message, score);
            }
        }

        void ShowHud()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score " + score;
            }

            if (timeText != null)
            {
                int seconds = Mathf.CeilToInt(timeLeft);
                int minutes = seconds / 60;
                timeText.text = minutes + ":" + (seconds % 60).ToString("00");
            }
        }
    }
}