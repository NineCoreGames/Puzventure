using UnityEngine;
using TMPro;

public class E_Round : MonoBehaviour
{
    // turun süresi ve iki HUD yazısı
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    // bitiş ekranı
    [SerializeField] private E_GameOver gameOver;

    private int score;       // bu turdaki puan
    private float timeLeft;  // kalan saniye
    private bool over;       // tur bitti mi

    void Start()
    {
        timeLeft = roundLength;  // saati doldur
    }

    void Update()
    {
        if (over) return;  // bittiyse hiçbir şey değişmesin

        // geri say, ama 0'ın altına inme
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();
        if (timeLeft <= 0f) EndRound("Time's up!");
    }

    // Altar bunu çağırır
    public void AddPoints(int points)
    {
        if (over) return;
        score += points;
    }

    // süre bitince turu bitir
    public void EndRound(string message)
    {
        if (over) return;
        over = true;
        Time.timeScale = 0f;  // dünyayı durdur
        gameOver.Show(message, score);
    }

    // iki sayıyı ekrana yaz
    void ShowHud()
    {
        scoreText.text = "Score " + score;
        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}