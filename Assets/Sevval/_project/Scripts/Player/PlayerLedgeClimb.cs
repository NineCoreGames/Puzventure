
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class PlayerLedgeClimb : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public MonoBehaviour playerMove;
    public MonoBehaviour playerJump;

    [Header("Detection")]
    public float wallCheckDistance = 0.8f;
    public float minLedgeHeight = 0.5f;
    public float maxLedgeHeight = 2.5f;
    public float topCheckForward = 0.8f;

    [Header("Climbing")]
    public float hangOffset = 0.15f;
    public float climbForward = 0.5f;
    public float climbDuration = 0.6f;
    public KeyCode climbKey = KeyCode.E;
    public KeyCode releaseKey = KeyCode.Q;

    private bool isHanging;
    private bool isClimbing;

    private Vector3 hangPosition;
    private Vector3 climbPosition;

    private float halfHeight;
    private float radius;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            halfHeight = col.bounds.extents.y;
            radius = Mathf.Min(
                col.bounds.extents.x,
                col.bounds.extents.z
            );
        }
        else
        {
            halfHeight = 0.9f;
            radius = 0.3f;
        }
    }
    void Update()
    {
        if (isClimbing)
            return;

        if (isHanging)
        {
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartCoroutine(ClimbUp());
            }

            if (Keyboard.current != null &&
                Keyboard.current.qKey.wasPressedThisFrame)
            {
                ReleaseLedge();
            }

            return;
        }

        TryGrabLedge();
    }

    void TryGrabLedge()
    {
        if (rb == null || rb.isKinematic)
            return;

        // Duvarın üst kısmına doğru ray gönder.
        Vector3 wallOrigin =
            transform.position + Vector3.up * halfHeight * 0.65f;

        if (!Physics.Raycast(
            wallOrigin,
            transform.forward,
            out RaycastHit wallHit,
            wallCheckDistance))
            return;

        // Duvarın üst yüzeyini bul.
        Vector3 topOrigin =
            transform.position
            + transform.forward * topCheckForward
            + Vector3.up * maxLedgeHeight;

        if (!Physics.Raycast(
            topOrigin,
            Vector3.down,
            out RaycastHit topHit,
            maxLedgeHeight + 0.5f))
            return;

        // Yalnızca yukarı bakan yüzeyleri kabul et.
        if (Vector3.Dot(topHit.normal, Vector3.up) < 0.7f)
            return;

        float ledgeHeight =
            topHit.point.y - transform.position.y;

        if (ledgeHeight < minLedgeHeight ||
            ledgeHeight > maxLedgeHeight)
            return;

        // Kenarın ön tarafında, asılı kalma konumu.
        hangPosition =
            topHit.point
            + wallHit.normal * (radius + 0.05f)
            + Vector3.up * (hangOffset - halfHeight);

        // Platformun üstünde durulacak konum.
        climbPosition =
            topHit.point
            - wallHit.normal * (radius + climbForward)
            + Vector3.up * (halfHeight + 0.05f);

        StartHanging();
    }

    void StartHanging()
    {
        isHanging = true;

        if (playerMove != null)
            playerMove.enabled = false;

        if (playerJump != null)
            playerJump.enabled = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.position = hangPosition;
    }

    IEnumerator ClimbUp()
    {
        isClimbing = true;

        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < climbDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / climbDuration);
            float smoothT = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(
                startPosition,
                climbPosition,
                smoothT
            );

            yield return null;
        }

        transform.position = climbPosition;

        FinishClimbing();
    }

    void FinishClimbing()
    {
        isHanging = false;
        isClimbing = false;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (playerMove != null)
            playerMove.enabled = true;

        if (playerJump != null)
            playerJump.enabled = true;
    }

    void ReleaseLedge()
    {
        isHanging = false;

        rb.isKinematic = false;

        if (playerMove != null)
            playerMove.enabled = true;

        if (playerJump != null)
            playerJump.enabled = true;
    }
}
