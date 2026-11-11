
using UnityEngine;
using TMPro;

namespace Sevval
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private TMP_Text inventoryText;
        [SerializeField] private TMP_Text keyText;

        void Update()
        {
            if (inventory == null || inventoryText == null)
                return;

            inventoryText.text =
                "Wood: " + inventory.WoodCount + "\n" +
                "Stone: " + inventory.StoneCount + "\n" +
                "Potion: " + inventory.PotionCount;

            keyText.text = inventory.HasKey ? "Anahtar: Var" : "Anahtar: Yok";
        }
    }
}
