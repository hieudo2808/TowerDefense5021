using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    [SerializeField] private Transform textDmgSpawnPosition;

    private Enemy _enemy;

    void Start()
    {
        _enemy.GetComponent<Enemy>();    
    }

    public void EnemyHit(Enemy enemy, float damage)
    {
        Debug.Log("Enemy Hit");
        if (_enemy == enemy)
        {
            GameObject newInstance;
        }
    }
}
