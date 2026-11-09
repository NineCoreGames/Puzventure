
using UnityEngine;

namespace Sevval {
    public class WaterfallEffect : MonoBehaviour
    {
        public float speed = 1f;

        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
        }

        void Update()
        {
            Vector2 offset = rend.material.mainTextureOffset;

            offset.y += speed * Time.deltaTime;

            rend.material.mainTextureOffset = offset;
        }
    }
}

