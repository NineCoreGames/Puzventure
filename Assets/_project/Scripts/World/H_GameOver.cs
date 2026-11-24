using UnityEngine;
using UnityEngine.SceneManagement; // Sahneyi yeniden yüklemek (Tekrar Oyna) için gerekli[cite: 288, 289]
using TMPro; // Ekrana yazý yazdýrmak için gerekli[cite: 288, 289]

public class H_GameOver : MonoBehaviour
{ 
    // Ortadaki büyük sonuç yazýsýný (ResultText) buraya baðlayacaðýz[cite: 288, 289]
    [SerializeField] private TMP_Text resultText;

    // H_Round scripti, süre bittiðinde bu fonksiyonu çaðýracak[cite: 288, 289]
    public void Show(string message, int score)
    {
        // Bilgisayara kaydedilmiþ olan "En Ýyi Skoru (Best)" oku. (Yoksa 0 kabul et)[cite: 288, 289]
        int best = PlayerPrefs.GetInt("best", 0);

        // Eðer bu turdaki skor, en iyiden büyükse yeni en iyi skoru güncelle[cite: 288, 290]
        if (score > best) best = score;
        PlayerPrefs.SetInt("best", best); // Bilgisayara kaydet[cite: 288, 290]

        // Ekrana 3 satýr halinde mesajý, skoru ve en iyi skoru yazdýr (\n = alt satýra geç)[cite: 288, 290]
        resultText.text = message + "\nScore " + score + "\nBest " + best;

        // Bu paneli (GameOverPanel) görünür yap[cite: 288, 290]
        gameObject.SetActive(true);
    }

    // Play Again (Tekrar Oyna) butonuna basýldýðýnda bu fonksiyon çalýþacak[cite: 288, 290]
    public void Restart()
    {
        // Zamaný yeniden baþlat (Çünkü oyun bittiðinde zamaný durduracaðýz)[cite: 303]
        Time.timeScale = 1f;

        // Þu anki sahneyi (Main) baþtan aþaðý yeniden yükle (Her þey sýfýrlanýr)[cite: 288, 290, 303]
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 