using UnityEngine;

[CreateAssetMenu(menuName = "Rune Vault/Cargo Type")]
public class E_CargoType : ScriptableObject
{
    [SerializeField] private int points = 1;
    [SerializeField] private float weight = 0f;

    public int Points()
    {
        return points;
    }

    public float Weight()
    {
        return weight;
    }
}