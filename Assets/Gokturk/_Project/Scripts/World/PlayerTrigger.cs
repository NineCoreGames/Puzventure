using UnityEngine;


namespace Puzventure.Gokturk
{
    public class PlayerTrigger : MonoBehaviour
{
    // set both in the Inspector, for each object
    [SerializeField] private string message = "Collected!";
    [SerializeField] private bool removeAfter = true;
    void OnTriggerEnter(Collider other)
    {
        // Who? only the object tagged "Player"
        if (!other.CompareTag("Player")) return;
        // Act: report it, remove it if asked
        Debug.Log(message);
        if (removeAfter) Destroy(gameObject);
    }
}
}
