using UnityEngine;
using TMPro;

public class N_Round : MonoBehaviour
{
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private N_GameOver gameOver;

    private int score;
    private float timeLeft;
    private bool over;

    void Start()
    {
        timeLeft = roundLength;
    }

    void Update()
    {
        if (over) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        ShowHud();
        if (timeLeft <= 0f) EndRound("Time's up!");
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

    void ShowHud()
    {
        scoreText.text = "Score " + score;
        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}