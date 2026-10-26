using UnityEngine;

namespace Puzventure.Rana
{
    public class ExplorerCarry : MonoBehaviour
    {
        [Header("Taþýma Noktasý")]
        [SerializeField] private Transform carryPoint;

        // Þu an taþýnan rün taþý (boþsa null)
        private RuneStone carriedRune;

        // Kaþifin eli dolu mu?
        public bool IsCarrying()
        {
            return carriedRune != null;
        }

        // Tuzaklara basýnca rünü düþür
        public void DropRune()
        {
            if (!IsCarrying()) return;

            carriedRune.Drop();
            carriedRune = null;
        }

        // Sunaða gelindiðinde rünü teslim et
        public RuneStone DeliverRune()
        {
            RuneStone rune = carriedRune;
            carriedRune = null;
            return rune;
        }

        // Bir trigger alanýna girdiðimizde
        private void OnTriggerEnter(Collider other)
        {
            // Temas edilen obje bir rün taþý mý?
            RuneStone rune = other.GetComponent<RuneStone>();
            if (rune == null) return;

            // Kaþifin eli zaten doluysa ikinci rünü alamaz
            if (IsCarrying()) return;

            // Az önce düþürüldüyse hemen geri kapýlmasýný engelle (bekleme süresi)
            if (!rune.CanBePickedUp()) return;

            // Rünü al ve baþýnýn üstüne baðla
            carriedRune = rune;
            rune.PickUp(carryPoint);
        }
    }
}
