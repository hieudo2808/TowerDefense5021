using TMPro;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    [SerializeField] private Transform textDmgSpawnPosition;

    private Enemy _enemy;

    void Start()
    {
        _enemy = GetComponent<Enemy>();    
    }

    public void EnemyHit(Enemy enemy, float damage)
    {
        Debug.Log("Enemy Hit");
        if (_enemy == enemy)
        {
            GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool();
            TextMeshProUGUI damageText = newInstance.GetComponent<DamageTextManager>().DmgText;
            damageText.text = damage.ToString();

            newInstance.transform.SetParent(textDmgSpawnPosition);
            newInstance.transform.position = textDmgSpawnPosition.position;
            newInstance.SetActive(true);
        }
    }

    private void OnEnable()
    {
        Projectile.OnEnemyHit += EnemyHit;
    }

    private void OnDisable()
    {
        Projectile.OnEnemyHit -= EnemyHit;
    }
}
