using UnityEngine;

public class Slippery : MonoBehaviour
{
    // 1 = normal zemin tutuşu, 0.04 = buz (neredeyse hiç tutuş yok)
    [SerializeField] private float grip = 0.04f;

    void OnCollisionEnter(Collision hit)
    {
        PlayerMove move = hit.collider.GetComponent<PlayerMove>();
        if (move != null) move.SetGrip(grip);
    }

    void OnCollisionExit(Collision hit)
    {
        PlayerMove move = hit.collider.GetComponent<PlayerMove>();
        if (move != null) move.SetGrip(1f);   // buzdan çıkınca normal tutuş
    }
}