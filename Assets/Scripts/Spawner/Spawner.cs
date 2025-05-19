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
    private ObjectPooler[] _pooler;
    // Start is called before the first frame update
    void Start()
    {
        _pooler = GetComponents<ObjectPooler>();
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
        newInstance.SetActive(true);
    }
}
