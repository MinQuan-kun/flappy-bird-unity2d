using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Cài đặt sinh ống")]
    public GameObject pipePrefab;
    public float spawnRate = 1.6f; // Khoảng cách thời gian giữa 2 cặp ống (giây)
    public float minY = -1.2f;     // Độ cao thấp nhất của tâm cặp ống
    public float maxY = 2.4f;      // Độ cao cao nhất của tâm cặp ống

    private float timer = 0f;

    void Update()
    {
        if (pipePrefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnPipe();
            timer = 0f;
        }
    }

    void SpawnPipe()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0f);
        Instantiate(pipePrefab, spawnPos, Quaternion.identity);
    }
}