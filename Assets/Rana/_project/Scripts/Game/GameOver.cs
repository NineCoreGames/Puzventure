using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzventure.Rana
{
    public class GameOver : MonoBehaviour
    {
        // Panelin ortasýndaki büyük sonuç metni
        [SerializeField] private TMP_Text resultText;

        // Süre bittiðinde Round bu metodu çaðýrýr
        public void Show(string message, int score)
        {
            // En yüksek skor bilgisayarda PlayerPrefs içinde saklanýr
            int best = PlayerPrefs.GetInt("best", 0);
            if (score > best) best = score;
            PlayerPrefs.SetInt("best", best);

            if (resultText != null)
            {
                resultText.text = message + "\nScore " + score + "\nBest " + best;
            }

            // Paneli görünür yap
            gameObject.SetActive(true);
        }

        // Play again butonu bu metodu çaðýracak (Step 15'te baðlanacak)
        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}