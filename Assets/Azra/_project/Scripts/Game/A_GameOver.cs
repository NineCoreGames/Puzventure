using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class A_GameOver : MonoBehaviour
{
    // panelin ortasindaki buyuk yazi
    [SerializeField] private TMP_Text resultText;

    // Round, tur bitince bunu cagirir
    public void Show(string message, int score)
    {
        // en iyi skor bu bilgisayarda saklanir.
        // anahtar "Azra_best": birlesik oyunda arkadaslarin "best"i ile karismasin
        int best = PlayerPrefs.GetInt("Azra_best", 0);
        if (score > best) best = score;
        PlayerPrefs.SetInt("Azra_best", best);

        resultText.text = message + "\nScore " + score + "\nBest " + best;
        gameObject.SetActive(true);   // panel gorunur
    }

    // Play again butonu bunu cagirir (Adim 4)
    public void Restart()
    {
        // Round zamani durdurdu: once tekrar baslat
        Time.timeScale = 1f;
        // icinde oldugumuz sahneyi bir daha yukle: temiz bir tur
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}