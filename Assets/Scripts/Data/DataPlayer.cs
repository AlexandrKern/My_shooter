using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Оставшиеся деньги" + playerStats.money);
    }

    public static void AddMoney(int money)
    {
        playerStats.money += money;
        OnMoneyChanged?.Invoke(playerStats.money);
        Save() ;
    }
}
