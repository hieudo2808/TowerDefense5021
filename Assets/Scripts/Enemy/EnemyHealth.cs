using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;
    public static Action<Enemy> OnEnemyFullyDestroyed;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;
    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    public float CurrentHealth { get; set; }

    private Image _healthBar;
    private Enemy _enemy;

    void Start()
    {
        CreateHealthBar();
        CurrentHealth = initialHealth;

        _enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(barPosition);
        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        _healthBar = container.FillAmountImage;
    }

    public void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(_enemy);
        }
    }

    private void Die()
    {
        OnEnemyKilled?.Invoke(_enemy);
        //ObjectPooler.ReturnToPool(gameObject);
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        _healthBar.fillAmount = 1f;
        _enemy.SetFirstPoint();
    }
}