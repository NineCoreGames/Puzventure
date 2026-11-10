using UnityEngine;

namespace Puzventure.Elif
{
    public class Spinner : MonoBehaviour
    {
        // degrees per SECOND
        [SerializeField] private float rotationSpeed = 90f;
        void Update()
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}
