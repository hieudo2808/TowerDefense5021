using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> _enemies = new List<Enemy>();
    private int attackRange = 3;

    public Enemy CurrentEnemyTarget;
    private float rotationSpeed = 100f;
    public float CurrentAngleToTarget { get; private set; }
    public bool IsAimed => CurrentAngleToTarget <= 0.1f;

    public int AttackRange => attackRange;
    [SerializeField] private int _turretCost;
    public int TurretCost => _turretCost;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = attackRange;
    }

    void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            _enemies.Add(newEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }

    private void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            CurrentEnemyTarget = null;
            return;
        }
        CurrentEnemyTarget = _enemies[0];
    }

    private void RotateTowardsTarget()
    {
        if (CurrentEnemyTarget == null)
        {
            return;
        }
        Vector3 direction = CurrentEnemyTarget.bodyAim.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        CurrentAngleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
    }
}