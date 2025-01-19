using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyManager _enemyManager;

    [SerializeField] private Transform[] _spawnPoints; 

    private bool _isSpawning = true;

    [System.Serializable]
    private class EnemySpawnData
    {
        public GameObject enemyPrefab; 
        public float spawnInterval;   
        [HideInInspector] public float currentTimer; 
    }

    [SerializeField] private EnemySpawnData[] _enemySpawnSettings; 

    private void Start()
    {
        _enemyManager = EnemyManager.Instace;

        foreach (var enemyData in _enemySpawnSettings)
        {
            enemyData.currentTimer = enemyData.spawnInterval;
        }
    }

    private void Update()
    {
        if (!_isSpawning) return;
        foreach (var enemyData in _enemySpawnSettings)
        {
            enemyData.currentTimer -= Time.deltaTime;

            if (enemyData.currentTimer <= 0)
            {
                SpawnEnemy(enemyData);
                enemyData.currentTimer = enemyData.spawnInterval; 
            }
        }
    }

    private void SpawnEnemy(EnemySpawnData enemyData)
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)]; 
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity); 
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();

        if (enemyBase != null)
        {
            _enemyManager.AddEnemy(enemyBase);
        }
    }
    public void ToggleIsSpawned(bool isSpawning)
    {
        _isSpawning = isSpawning;
    }
}

