
using UnityEngine;
using UnityEngine.UI;

namespace Sevval
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Slider healthSlider;

        void Start()
        {
            healthSlider.maxValue = playerHealth.MaxHealth;
        }

        void Update()
        {
            healthSlider.value = playerHealth.CurrentHealth;
        }
    }
}
