using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Sevval {
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultText;

        public void Show()
        {
            resultText.text = "GAME OVER";
            gameObject.SetActive(true);
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
