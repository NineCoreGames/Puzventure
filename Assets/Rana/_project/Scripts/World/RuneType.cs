using UnityEngine;

namespace Puzventure.Rana
{
    [CreateAssetMenu(menuName = "Puzventure/Rune Type")]
    public class RuneType : ScriptableObject
    {
        [SerializeField] private int points = 1;      // Teslimat puaný
        [SerializeField] private float weight = 0f;   // Karakter sýrtýna binecek ekstra aðýrlýk (kg)

        public int Points()
        {
            return points;
        }

        public float Weight()
        {
            return weight;
        }
    }
}