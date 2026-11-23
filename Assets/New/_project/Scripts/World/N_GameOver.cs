using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class N_GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;

    public void Show(string message, int score)
    {
        int best = PlayerPrefs.GetInt("best", 0);
        if (score > best) best = score;
        PlayerPrefs.SetInt("best", best);

        resultText.text = message + "\nScore " + score + "\nBest " + best;
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}