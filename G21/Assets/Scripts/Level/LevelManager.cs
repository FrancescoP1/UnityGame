using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int lives = 10;

    public int totalLives { get; set; }
    public int currentWave { get; set; }

    private void Start()
    {
        totalLives = lives;
        currentWave = 1;
    }

    private void ReduceLives(Enemy enemy)
    {
        totalLives--;
        if (totalLives <= 0) 
        {
            totalLives = 0;
            GameOver();
        }
    }

    private void GameOver() 
    {
        // Debug.Log("GAME OVER!");
    }

    private void WaveCompleted() 
    {

    }

    private void OnEnable()
    {
        Enemy.OnEndReached += ReduceLives;
        // Spawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= ReduceLives;
        // Spawner.OnWaveCompleted -= WaveCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(totalLives);
    }
}
