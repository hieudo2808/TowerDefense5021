using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int enemyCount = 10;

    [Header("Fixed Delay")]
    [SerializeField]
    private float delayBtwSpawns;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRamaining;
    private ObjectPooler[] _pooler;
    private Waypoint _waypoint;

    public static Action OnWaveCompleted;

    void Start()
    {
        _waypoint = GetComponent<Waypoint>();
        _pooler = GetComponents<ObjectPooler>();

        _enemiesRamaining = enemyCount;
    }
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0) 
        {
            _spawnTimer = delayBtwSpawns;
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = _pooler[UnityEngine.Random.Range(0, _pooler.Length - 1)].GetInstanceFromPool();
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoints = _waypoint;
        enemy.SetFirstPoint();

        newInstance.SetActive(true);
    }

    private void RecordEnemy(Enemy enemy)
    {
        _enemiesRamaining--;
        if (_enemiesRamaining <= 0)
        {
            OnWaveCompleted?.Invoke();
        }
    }    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyFullyDestroyed += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyFullyDestroyed -= RecordEnemy;
    }
}
