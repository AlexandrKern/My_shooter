using System;
using System.IO;
using UnityEngine;

public static class DataPlayer 
{
    private static string _path = Path.Combine(Application.persistentDataPath, "DataPlayer.json");
    private static PlayerStats playerStats;

    public static event Action<int> OnMoneyChanged;

    public static void Load()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            playerStats = JsonUtility.FromJson<PlayerStats>(json);
        }
        else
        {
            playerStats = new PlayerStats();
            Save();
        }
    }

    public static void Save()
    {
        string data = JsonUtility.ToJson(playerStats,true);
        File.WriteAllText(_path, data);
    }

    public static int GetMoney()
    {
        return playerStats.money;
    } 

    public static void SubstractMoney(int money)
    {
        playerStats.money -= money;
        OnMoneyChanged?.Invoke(playerStats.money);
        Save();
    }

    public static void AddMoney(int money)
    {
        playerStats.money += money;
        OnMoneyChanged?.Invoke(playerStats.money);
        Save() ;
    }

    public static void AddEnemyDeathCount(EnemyBase enemy)
    {
        if(enemy is Warrok warrok)
        {
            playerStats.warroktDeathCount++;
        }
        if (enemy is Mutant mutant)
        {
            playerStats.mutantDeathCount++;
        }
        if (enemy is Vampire vampire)
        {
            playerStats.vampireDeathCount++;
        }
        Save() ;
    }

    public static int  GetEnemyDeathCount(string enemyName)
    {
        switch (enemyName)
        {
            case "warrok":
                return playerStats.warroktDeathCount;
            case "mutant":
                return playerStats.mutantDeathCount;
            case "vampire":
                return playerStats.vampireDeathCount;
            default:
                return 0;
        }
    }

    public static void SetWaveMaxCount(int waveCount)
    {
        if(playerStats.maxWave < waveCount-1)
        {
            playerStats.maxWave = waveCount-1;
        }
    }

    public static int GetWaveMaxCount()
    {
        return playerStats.maxWave;
    }
}
