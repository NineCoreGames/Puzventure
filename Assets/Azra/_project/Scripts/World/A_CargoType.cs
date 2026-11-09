using UnityEngine;

// Create menusune Azra > Cargo Type ekler
[CreateAssetMenu(menuName = "Azra/Cargo Type")]
public class A_CargoType : ScriptableObject
{
    [SerializeField] private int points = 1;      // teslim edilince kac puan
    [SerializeField] private float weight = 0f;   // oyuncuya eklenen kutle (kg)

    // baska script'ler okuyabilir, degistiremez (W02 kurali)
    public int Points()
    {
        return points;
    }

    public float Weight()
    {
        return weight;
    }
}