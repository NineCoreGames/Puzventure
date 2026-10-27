
using UnityEngine;

namespace Sevval
{

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private string message = "Collected!";
    [SerializeField] private bool removeAfter = true;

    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        Debug.Log(message);

        if (removeAfter)
        {
            Destroy(gameObject);
        }
    }
}

}
