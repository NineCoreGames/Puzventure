using UnityEngine;

namespace Berfin
{
    [CreateAssetMenu(menuName = "Rune Vault/Rune Type")]
    public class RuneType : ScriptableObject
    {
        [Header("Delivery Reward")]
        [SerializeField] private int points = 1;

        [Header("Physical Cost")]
        [Tooltip("Taşırken karaktere binecek ek kütle (kg)")]
        [SerializeField] private float weight = 0f;

        public int Points => points;
        public float Weight => weight;
    }
}