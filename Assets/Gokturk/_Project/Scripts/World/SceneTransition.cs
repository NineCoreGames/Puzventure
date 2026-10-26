using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes


namespace Puzventure.Gokturk
{
    public class SceneTransition : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string sceneToLoad; // Type the exact name of your next scene here

    // Use OnTriggerEnter for 3D games, or OnTriggerEnter2D for 2D games
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the door is tagged as the Player
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // Loads the specified scene
        }
        else
        {
            Debug.LogWarning("Please specify a scene name in the Door script inspector!");
        }
    }
}
}

