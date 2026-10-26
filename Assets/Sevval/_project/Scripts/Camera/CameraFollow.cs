using UnityEngine;


    public class CameraFollow : MonoBehaviour
    {
        public Transform player;

        public Vector3 offset = new Vector3(0f, 2.5f, -4f);

        public float positionSmooth = 10f;
        public float rotationSmooth = 15f;

        void LateUpdate()
        {
            if (player == null) return;

            // Kameranın oyuncunun dönüşüne göre konumu
            Vector3 targetPosition =
                player.position + player.rotation * offset;

            // Kamerayı yumuşak şekilde takip ettir
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                positionSmooth * Time.deltaTime
            );

            // Kamerayı oyuncuya yönelt
            Vector3 lookTarget =
                player.position + Vector3.up * 1.2f;

            Quaternion targetRotation = Quaternion.LookRotation(
                lookTarget - transform.position
            );

            transform.rotation = targetRotation;
        }
    }

