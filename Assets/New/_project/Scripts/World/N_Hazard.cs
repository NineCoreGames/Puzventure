using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class N_Hazard : MonoBehaviour
{
    [SerializeField] private bool instantDeath = false;

    private CinemachineImpulseSource shake;
    private bool restarting = false;

    void Start()
    {
        shake = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        HandleHazard(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleHazard(collision.gameObject);
    }

    void HandleHazard(GameObject player)
    {
        if (!player.CompareTag("Player")) return;
        if (restarting) return;

        N_PlayerCarry carry = player.GetComponent<N_PlayerCarry>();

        if (shake != null)
            shake.GenerateImpulse();

        if (!instantDeath && carry != null && carry.IsCarrying())
        {
            carry.DropCargo();
            return;
        }

        restarting = true;

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        N_PlayerMove move = player.GetComponent<N_PlayerMove>();
        if (move != null) move.enabled = false;

        N_PlayerJump jump = player.GetComponent<N_PlayerJump>();
        if (jump != null) jump.enabled = false;

        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}