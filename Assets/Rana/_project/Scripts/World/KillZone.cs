using UnityEngine;

namespace Puzventure.Rana
{
    public class KillZone : MonoBehaviour
    {
        [Tooltip("Oyuncunun yeniden doðacaðý baþlangýç noktasý")]
        [SerializeField] private Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            // Eðer düþen obje oyuncu ise
            if (other.CompareTag("Player"))
            {
                // Karakterde CharacterController varsa çakýþmamasý için kapatýp açýyoruz
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                // Oyuncuyu spawn noktasýna taþý
                if (spawnPoint != null)
                {
                    other.transform.position = spawnPoint.position;
                    other.transform.rotation = spawnPoint.rotation;
                }

                if (cc != null) cc.enabled = true;
            }
        }
    }
}