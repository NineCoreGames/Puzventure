using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzventure.Gokturk
{
    public class Hazard : MonoBehaviour
{
    // solid hazard (player bumps into it)
    void OnCollisionEnter(Collision collision)
    {
        TryRestart(collision.collider);
    }

    // trigger hazard (player passes into it)
    void OnTriggerEnter(Collider other)
    {
        TryRestart(other);
    }

    private void TryRestart(Collider other)
    {
        // only react to the player, not other objects
        if (other.GetComponentInParent<PlayerMove>() == null) return;

        // reload the scene we are in right now
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
}
