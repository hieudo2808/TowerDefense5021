using UnityEngine;

public class CurrencySystem : Singleton<CurrencySystem>
{
    [SerializeField] private int coin = 60;
    private string CURRENCY_KEY = "CurrencyAmount";
    public int TotalCoins { get; set; }

    private void Start()
    {
        TotalCoins = coin;
        PlayerPrefs.DeleteKey(CURRENCY_KEY);
    }

    private void LoadCoins()
    {
        TotalCoins += PlayerPrefs.GetInt(CURRENCY_KEY, coin);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        PlayerPrefs.SetInt(CURRENCY_KEY, TotalCoins);
        PlayerPrefs.Save();
    }

    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(CURRENCY_KEY, TotalCoins);
            PlayerPrefs.Save();
        }        
    }

    private void AddCoins(Enemy enemy)
    {
        AddCoins(enemy.DeathCoinReward);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += AddCoins;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= AddCoins;
    }
}