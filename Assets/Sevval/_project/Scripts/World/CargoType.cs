
using UnityEngine;

namespace Sevval
{
    [CreateAssetMenu(menuName = "Puzventure/Cargo Type")]
    public class CargoType : ScriptableObject
    {
        [SerializeField] private int points = 1;
        [SerializeField] private float weight = 0f;

        [Header("Inventory")]
        [SerializeField] private bool countsAsStone = false;

        public int Points()
        {
            return points;
        }

        public float Weight()
        {
            return weight;
        }

        public bool CountsAsStone()
        {
            return countsAsStone;
        }
    }
}
