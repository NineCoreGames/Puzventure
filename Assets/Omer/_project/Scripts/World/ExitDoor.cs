using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameObject solidDoor; 
    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen || !other.CompareTag("Player")) return;

        PlayerStatus player = other.GetComponent<PlayerStatus>();
        
        if (player != null)
        {
            if (player.HasKey()) 
            {
                OpenDoor();
                player.UseKey(); 
            }
            else
            {
                Debug.Log("Kapı kilitli! Önce anahtarı bulmalısın.");
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        solidDoor.SetActive(false); 
        Debug.Log("Kapı açıldı! Bölüm tamamlandı!");
    }
}