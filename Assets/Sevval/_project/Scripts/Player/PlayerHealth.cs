
using UnityEngine;

namespace Sevval
{
    public class PlayerHealth : MonoBehaviour
    {
        public int MaxHealth = 100;
        public int CurrentHealth = 100;

        // Oyuncu hasar aldığında
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth < 0)
                CurrentHealth = 0;

            Debug.Log("Can: " + CurrentHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        // Oyuncu iyileştiğinde
        public void Heal(int amount)
        {
            CurrentHealth += amount;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;

            Debug.Log("Can: " + CurrentHealth);
        }

        // Oyuncu öldüğünde
        private void Die()
        {
            Debug.Log("Oyuncu öldü!");
        }
    }
}
