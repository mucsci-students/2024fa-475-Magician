using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundEnemyActions;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject[] __enemyPrefabs;  // Array of different enemy types
    [SerializeField] Transform _spawnArea;        // Reference to the area where enemies should spawn
    [SerializeField] float _spawnAreaWidth = 10f; // Width of spawn area
    [SerializeField] float _spawnAreaHeight = 5f; // Height of spawn area
    [SerializeField] int _firstEnemyNum = 1;
    [SerializeField] int _secondEnemyNum = 5;
    [SerializeField] float _smallEnemyHealth = 100f;
    [SerializeField] float _mediumEnemyHealth = 150f;
    [SerializeField] float _bigEnemyHealth = 150f;
    [SerializeField] float _smallEnemySpeed = 1f;
    [SerializeField] float _mediumEnemySpeed = 0.8f;
    [SerializeField] float _bigEnemySpeed = 0.8f;
    [SerializeField] int _smallEnemyDamage = 10;
    [SerializeField] int _mediumEnemyDamage = 20;
    [SerializeField] int _bigEnemyDamage = 20;
    [SerializeField] float _bossHealth = 1000f;
    [SerializeField] float _bossSpeed = 0.5f;
    [SerializeField] float _delayTime = 10f;
    [SerializeField] int _bossDamage = 50;

    private List<EnemyData> enemyList = new List<EnemyData>();
    private int _maxEnemies = 0;

    void Start()
    {
        // Spawn the initial enemies
        SpawnEnemies();

        // Start the coroutine to check for enemy deaths and respawn
        StartCoroutine(CheckForRespawn());
    }

    void SpawnEnemies()
    {
        foreach (GameObject _enemyPrefab in __enemyPrefabs)
        {
            // Generate a random number between _firstEnemyNum and _secondEnemyNum
            int enemyCount = Random.Range(_firstEnemyNum, _secondEnemyNum + 1);

            _maxEnemies += enemyCount;

            // Spawn the specified number of enemies of this type
            for (int i = 0; i < enemyCount; i++)
            {
                Vector2 randomPosition = GetRandomSpawnPosition();
                GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomPosition, Quaternion.identity);

                // Now modify the stats of the spawned enemy based on its tag
                EnemyStats enemyStat = spawnedEnemy.GetComponent<EnemyStats>();

                if (enemyStat != null)
                {
                    // Check the enemy tag and apply the appropriate stats
                    if (spawnedEnemy.CompareTag("SmallEnemy"))
                    {
                        enemyStat.SetEnemyHealth(_smallEnemyHealth);
                        enemyStat.SetEnemyDamage(_smallEnemyDamage);
                        enemyStat.SetEnemySpeed(_smallEnemySpeed);
                    }
                    else if (spawnedEnemy.CompareTag("MediumEnemy"))
                    {
                        enemyStat.SetEnemyHealth(_mediumEnemyHealth);
                        enemyStat.SetEnemyDamage(_mediumEnemyDamage);
                        enemyStat.SetEnemySpeed(_mediumEnemySpeed);
                    }
                    else if (spawnedEnemy.CompareTag("BigEnemy"))
                    {
                        enemyStat.SetEnemyHealth(_bigEnemyHealth);
                        enemyStat.SetEnemyDamage(_bigEnemyDamage);
                        enemyStat.SetEnemySpeed(_bigEnemySpeed);
                    }
                    else if (spawnedEnemy.CompareTag("Boss"))
                    {
                        enemyStat.SetEnemyHealth(_bossHealth);
                        enemyStat.SetEnemyDamage(_bossDamage);
                        enemyStat.SetEnemySpeed(_bossSpeed);
                    }
                }

                // Add the enemy to the list for tracking
                enemyList.Add(new EnemyData(spawnedEnemy, _enemyPrefab, spawnedEnemy.tag));
            }
        }
    }

    IEnumerator CheckForRespawn()
    {
        while (true)
        {
            // Wait for some time between checks
            yield return new WaitForSeconds(1f);

            // Loop through the enemy list
            foreach (EnemyData enemyData in enemyList)
            {
                if (enemyData._enemyInstance == null && !enemyData._isRespawning)
                {
                    // The enemy has been destroyed, start respawn coroutine
                    StartCoroutine(RespawnEnemyAfterDelay(enemyData, _delayTime)); // 5 minutes
                    enemyData._isRespawning = true;
                }
            }
        }
    }

    IEnumerator RespawnEnemyAfterDelay(EnemyData enemyData, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector2 randomPosition = GetRandomSpawnPosition();
        GameObject spawnedEnemy = Instantiate(enemyData._enemyPrefab, randomPosition, Quaternion.identity);

        // Now modify the stats of the spawned enemy based on its tag
        EnemyStats enemyStat = spawnedEnemy.GetComponent<EnemyStats>();

        if (enemyStat != null)
        {
            // Check the enemy tag and apply the appropriate stats
            if (spawnedEnemy.CompareTag("SmallEnemy"))
            {
                enemyStat.SetEnemyHealth(_smallEnemyHealth);
                enemyStat.SetEnemyDamage(_smallEnemyDamage);
                enemyStat.SetEnemySpeed(_smallEnemySpeed);
            }
            else if (spawnedEnemy.CompareTag("MediumEnemy"))
            {
                enemyStat.SetEnemyHealth(_mediumEnemyHealth);
                enemyStat.SetEnemyDamage(_mediumEnemyDamage);
                enemyStat.SetEnemySpeed(_mediumEnemySpeed);
            }
            else if (spawnedEnemy.CompareTag("Boss"))
            {
                enemyStat.SetEnemyHealth(_bossHealth);
                enemyStat.SetEnemyDamage(_bossDamage);
                enemyStat.SetEnemySpeed(_bossSpeed);
            }
        }

        // Update the enemy instance in the list and reset _isRespawning
        enemyData._enemyInstance = spawnedEnemy;
        enemyData._isRespawning = false;
    }

    Vector2 GetRandomSpawnPosition()
    {
        // Generate random position within the defined spawn area
        float randomX = Random.Range(-_spawnAreaWidth / 2, _spawnAreaWidth / 2);
        float randomY = Random.Range(-_spawnAreaHeight / 2, _spawnAreaHeight / 2);
        Vector2 spawnPosition = (Vector2)_spawnArea.position + new Vector2(randomX, randomY);
        return spawnPosition;
    }

    // Class to keep track of enemy data
    private class EnemyData
    {
        public GameObject _enemyInstance;
        public GameObject _enemyPrefab;
        public string _enemyTag;
        public bool _isRespawning;

        public EnemyData(GameObject instance, GameObject prefab, string tag)
        {
            _enemyInstance = instance;
            _enemyPrefab = prefab;
            _enemyTag = tag;
            _isRespawning = false;
        }
    }
}
