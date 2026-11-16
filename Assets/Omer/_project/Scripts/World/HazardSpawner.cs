using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hazardPrefab; 
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private float minX = -2f; 
    [SerializeField] private float maxX = 2f;  
    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHazard();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnHazard()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        Instantiate(hazardPrefab, spawnPosition, transform.rotation);
    }
}