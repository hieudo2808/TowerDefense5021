using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int lives = 1;
    public int TotalLives { get; set; }
    //public int CurrentWave { get; set; }

    private void Start()
    {
        TotalLives = lives;
        //CurrentWave = 1;
    }

    private void ReduceLives(Enemy enemy)
    {
        TotalLives--;
        if (TotalLives <= 0)
        {
            TotalLives = 0;
            GameOver();
        }
    }

    private void GameWin()
    {
        UIManager.Instance.ShowWinPanel();
    }

    private void GameOver()
    {
    }
    private void WaveCompleted()
    {
        //CurrentWave++;
        GameWin();
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += ReduceLives;
        Spawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= ReduceLives;
        Spawner.OnWaveCompleted -= WaveCompleted;
    }
}