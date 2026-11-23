using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class E_GameOver : MonoBehaviour
{
    // panelin ortasındaki büyük yazı
    [SerializeField] private TMP_Text resultText;

    // E_Round tur bitince bunu çağırır
    public void Show(string message, int score)
    {
        // en iyi skor bu bilgisayarda saklanıyor
        int best = PlayerPrefs.GetInt("best", 0);
        if (score > best) best = score;
        PlayerPrefs.SetInt("best", best);

        resultText.text = message + "\nScore " + score + "\nBest " + best;
        gameObject.SetActive(true);  // panel görünsün
    }

    // Play again butonu bunu çağırır
    public void Restart()
    {
        // E_Round zamanı durdurmuştu: önce tekrar başlat
        Time.timeScale = 1f;
        // içinde olduğumuz sahneyi yeniden yükle: yeni tur
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}