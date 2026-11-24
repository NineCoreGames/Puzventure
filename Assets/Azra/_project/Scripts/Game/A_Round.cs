using UnityEngine;
using TMPro;

public class A_Round : MonoBehaviour
{
    // turun suresi ve iki HUD yazisi
    [SerializeField] private float roundLength = 150f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    // kapi kurali (benim levelim)
    [SerializeField] private int scoreToOpen = 7;
    [SerializeField] private GameObject gate;

    // bitis ekrani ve zaman bonusu
    [SerializeField] private A_GameOver gameOver;
    [SerializeField] private float secondsPerBonusPoint = 10f;   // kalan her 10 sn = +1 puan

    private int score;        // bu turdaki puan
    private float timeLeft;   // bitise kalan saniye
    private bool gateOpen;    // kapi acildi mi?
    private bool over;        // tur bittiyse true

    void Start()
    {
        timeLeft = roundLength;   // saat dolu baslar
    }

    void Update()
    {
        if (over) return;     // bittiyse artik hicbir sey degismez

        // geri say, ama sifirin altina inme
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();
        if (timeLeft <= 0f) EndRound("Time's up!");
    }

    // teslim alani bunu cagirir
    public void AddPoints(int points)
    {
        if (over) return;     // bitisten sonra puan yok
        score += points;

        // 7'ye ulasti mi? kapi bir kere acilir
        if (!gateOpen && score >= scoreToOpen)
        {
            gateOpen = true;
            if (gate != null) gate.SetActive(false);
            Debug.Log("The gate is open!");
        }
    }

    // baska script'ler sorabilir: kapi acik mi?
    public bool GateOpen()
    {
        return gateOpen;
    }

    // oyuncu acik kapidan cikti - kalan sure bonus olur
    public void Escape()
    {
        if (over) return;
        int bonus = Mathf.FloorToInt(timeLeft / secondsPerBonusPoint);
        score += bonus;
        EndRound("You escaped!\nTime bonus +" + bonus);
    }

    // sure bitti ya da oyuncu kacti: turu bitir
    public void EndRound(string message)
    {
        if (over) return;          // iki kere bitirme
        over = true;
        Time.timeScale = 0f;       // dunyayi durdur
        gameOver.Show(message, score);
    }

    // iki sayiyi ekrana yaz
    void ShowHud()
    {
        if (gateOpen) scoreText.text = "Score " + score + " - Gate open!";
        else scoreText.text = "Score " + score + " / " + scoreToOpen;

        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}