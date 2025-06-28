using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public TextMeshProUGUI currentWave, enemiesLeft;
    public int startingEnemyCount = 10;
    public float baseInterval = 3f;
    public float intervalReduction = 0.2f;
    public float minInterval = 0.2f;

    private int wave = 0;
    private int enemiesRemaining;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            wave++;
            UpdateWaveCounter();

            int enemyCount = startingEnemyCount + (wave - 1) * 10;
            enemiesRemaining = enemyCount;
            float spawnInterval = Mathf.Max(minInterval, baseInterval - (wave - 1) * intervalReduction);

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                UpdateEnemyCounter();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Wait until all enemies are defeated before starting next wave
            yield return new WaitUntil(() => enemiesRemaining == 0);
            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab is not assigned");
            return;
        }
        Vector2 spawnPos = GetOffScreenSpawnPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetOffScreenSpawnPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
            return Vector2.zero;
        }

        Vector2 playerPos = player.transform.position;
        float spawnDistance = 10f;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 spawnOffset = randomDir * spawnDistance;
        Vector2 spawnPos = playerPos + spawnOffset;
        return spawnPos;
    }

    void UpdateWaveCounter()
    {
        if (currentWave != null)
        {
            currentWave.text = "Wave: " + wave;
        }
    }

    void UpdateEnemyCounter()
    {
        if (enemiesLeft != null)
        {
            enemiesLeft.text = "Enemies left: " + enemiesRemaining;
        }
    }

    public void DecreaseEnemyCount()
    {
        enemiesRemaining--;
        UpdateEnemyCounter();
    }
}
