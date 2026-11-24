using UnityEngine;
using TMPro;

public class A_Round : MonoBehaviour
{
    [SerializeField] private float roundLength = 150f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int scoreToOpen = 7;
    [SerializeField] private GameObject gate;
    [SerializeField] private A_GameOver gameOver;   // YENI: bitis ekrani

    private int score;
    private float timeLeft;
    private bool gateOpen;
    private bool over;        // YENI: tur bittiyse true

    void Start()
    {
        timeLeft = roundLength;
    }

    void Update()
    {
        if (over) return;     // YENI: bittiyse artik hicbir sey degismez

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();
        if (timeLeft <= 0f) EndRound("Time's up!");   // YENI: saat bitti
    }

    public void AddPoints(int points)
    {
        if (over) return;     // YENI: bitisten sonra puan yok
        score += points;

        if (!gateOpen && score >= scoreToOpen)
        {
            gateOpen = true;
            if (gate != null) gate.SetActive(false);
            Debug.Log("The gate is open!");
        }
    }

    public bool GateOpen()
    {
        return gateOpen;
    }

    // YENI: sure bitti - ya da baska bir script turu bitiriyor (Adim 5: kapidan cikis)
    public void EndRound(string message)
    {
        if (over) return;          // iki kere bitirme
        over = true;
        Time.timeScale = 0f;       // dunyayi durdur
        gameOver.Show(message, score);
    }

    void ShowHud()
    {
        if (gateOpen) scoreText.text = "Score " + score + " - Gate open!";
        else scoreText.text = "Score " + score + " / " + scoreToOpen;

        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}