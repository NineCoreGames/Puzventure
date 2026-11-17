using UnityEngine;

public class FrictionZone : MonoBehaviour //kaygan rampalarda karakterin zemin tutuşunu azaltır!

{
    [Range(0.01f, 1f)]
    [SerializeField] private float surfaceGrip = 0.05f;//kayganlık değeri

    private void OnCollisionEnter(Collision collision) //buzlu alana girilen ilk an
    {
        if (collision.collider.TryGetComponent<PlayerController>(out var player))
        {
            player.ModifyFriction(surfaceGrip);//playercontroller scriptimizdeki dışarıdan sürtünme değiştiren fonku çağırır.
        }
    }

    private void OnCollisionExit(Collision collision) //buzlu alandan çıkınca...
    {
        if (collision.collider.TryGetComponent<PlayerController>(out var player))
        {
            player.ModifyFriction(1f);//sürtünmeyi normale döndürür.
        }
    }
}