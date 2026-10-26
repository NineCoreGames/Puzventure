using UnityEngine;

namespace Puzventure.Omer
{
    public class Blinker : MonoBehaviour
    {
        [SerializeField] private float interval = 1.0f;   // saniye

        private Renderer rend;
        private float timer;

        public bool IsOn => rend.enabled;                 // Hazard buna bakacak

        void Start()
        {
            rend = GetComponentInChildren<Renderer>();    // Renderer kendinde ya da çocuğunda olabilir
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                rend.enabled = !rend.enabled;
                timer = 0f;
            }
        }
    }
}
