using UnityEngine;
using TMPro;

public class A_Round : MonoBehaviour
{
    // turun suresi ve iki HUD yazisi
    [SerializeField] private float roundLength = 150f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    // YENI (benim levelim): kapi kurali artik Round'da
    [SerializeField] private int scoreToOpen = 7;
    [SerializeField] private GameObject gate;

    private int score;        // bu turdaki puan
    private float timeLeft;   // bitise kalan saniye
    private bool gateOpen;    // kapi acildi mi?

    void Start()
    {
        timeLeft = roundLength;   // saat dolu baslar
    }

    void Update()
    {
        // geri say, ama sifirin altina inme
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();
    }

    // teslim alani bunu cagirir
    public void AddPoints(int points)
    {
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