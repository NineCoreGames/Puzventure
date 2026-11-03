using UnityEngine;

// Sarkac: Pivot objesine takilir, cocugu (ip + top) saga sola sallanir.
// A_Bobber ile ayni fikir (Mathf.Sin + Time.time), bu sefer konum yerine aci.
public class A_Swinger : MonoBehaviour
{
    [SerializeField] private float swingAngle = 60f;  // ortadan iki yana en fazla kac derece
    [SerializeField] private float swingSpeed = 2f;   // ne kadar hizli sallanir
    [SerializeField] private float timeOffset = 0f;   // toplar ayni anda sallanmasin diye

    void Update()
    {
        // Sinus dalgasi: -1 ... +1 arasi gider gelir -> -60 ... +60 derece
        float angle = Mathf.Sin((Time.time + timeOffset) * swingSpeed) * swingAngle;

        // X ekseni etrafinda don: top Z yonunde (koridorun enine) sallanir
        transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
}