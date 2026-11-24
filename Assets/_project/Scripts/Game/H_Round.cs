using UnityEngine;
using TMPro;

public class H_Round : MonoBehaviour
{
    [SerializeField] private float roundLength = 90f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    // YENÝ: Game Over panelini buraya baðlayacaðýz[cite: 292, 293]
    [SerializeField] private H_GameOver gameOver;

    private int score;
    private float timeLeft;

    // YENÝ: Oyunun bitip bitmediðini kontrol eden anahtar[cite: 292, 293]
    private bool over;

    void Start()
    {
        timeLeft = roundLength;
    }

    void Update()
    {
        if (over) return; // YENÝ: Oyun bittiyse artýk zamaný sayma[cite: 292, 293]

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        ShowHud();

        // YENÝ: Süre tamamen bittiðinde EndRound fonksiyonunu çaðýr[cite: 292, 293]
        if (timeLeft <= 0f) EndRound("Time's up!");
    }

    public void AddPoints(int points)
    {
        if (over) return; // YENÝ: Oyun bittiyse puan ekleme[cite: 292, 293]
        score += points;
    }

    // YENÝ: Oyun bitirme iþlemleri[cite: 292, 293]
    public void EndRound(string message)
    {
        if (over) return; // Zaten bittiyse tekrar bitirme[cite: 292, 293]
        over = true;

        Time.timeScale = 0f; // Tüm oyunu (zamaný, fiziði) dondur[cite: 292, 294]

        gameOver.Show(message, score); // Paneli göster ve puaný gönder[cite: 292, 293]
    }

    void ShowHud()
    {
        scoreText.text = "Score " + score;
        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
} 