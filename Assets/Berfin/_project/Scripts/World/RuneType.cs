using UnityEngine;

namespace Berfin
{
    [CreateAssetMenu(menuName = "Rune Vault/Rune Type")]
    public class RuneType : ScriptableObject  //farklı rün türlerini depolamak...
    {
        [Header("Delivery Reward")]
        [SerializeField] private int points = 1;

        [Header("Physical Cost")]
        [Tooltip("Taşırken karaktere binecek ek kütle (kg)")]
        [SerializeField] private float weight = 0f;

        public int Points => points;//rün teslim edilince oyuncunun kazanacağı puan
        public float Weight => weight;//rün alınınca oyuncuya yüklenecek ağırlık
        //=>(lambda): sadece okunur demek
    }
}

//ScriptableObject : sahnedeki objelere takılmayan sadece veri saklamak amacıyla oluşturulan özel bir veri tabanı varlığıdır.
//monobehavior: sahnedeki objelere takılır ve Update/Start gibi yaşam döngüleri vardır.