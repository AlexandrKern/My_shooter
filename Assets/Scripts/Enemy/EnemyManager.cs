using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager Instace;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<EnemyBase> _enemies = new List<EnemyBase>();
    [HideInInspector] public int enemyCount;
    private int warroktDeathCount;
    private int mutantDeathCount;
    private int vampireDeathCount;

    [SerializeField] private EnemyBase _mutant;
    [SerializeField] private EnemyBase _vampire;
    [SerializeField] private EnemyBase _warrok;

    [SerializeField] private int _healthGrowth;
    [SerializeField] private int _damageGrowth;


    public event Action<int> OnAddEnemy;
    public event Action<int> OnRemoveEnemy;

    private void Start()
    {
        SetEnemyDamage(DataPlayer.GetWaveMaxCount());
        SetEnemyHealth(DataPlayer.GetWaveMaxCount());
        warroktDeathCount = 0;
        mutantDeathCount = 0;
        vampireDeathCount = 0;
        SceneController.Instance.enemyWaveManager.OnCangeWaveCount += SetEnemyHealth;
        SceneController.Instance.enemyWaveManager.OnCangeWaveCount += SetEnemyDamage;
    }

    private void Update()
    {
        if(_enemies.Count <= 0) return;
        foreach (EnemyBase enemy in _enemies)
        {
            if (enemy.isDeath)
            {
                if(!enemy.isMove) return;
                enemy.Move();
                enemy.Attack();
            }
            
        }
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        _enemies.Remove(enemy);
        OnRemoveEnemy?.Invoke(_enemies.Count);
    } 
    public void AddEnemy(EnemyBase enemy)
    {
        
        _enemies.Add(enemy);
        enemyCount++;
        OnAddEnemy?.Invoke(enemyCount);
    }

    public void AddEnemyDeathCurrentCount(EnemyBase enemy)
    {
        if (enemy is Warrok warrok)
        {
            warroktDeathCount++;
        }
        if (enemy is Mutant mutant)
        {
            mutantDeathCount++;
        }
        if (enemy is Vampire vampire)
        {
            vampireDeathCount++;
        }
    }

    public int GetEnemyDeathCurrentCount(string enemyName)
    {
        switch (enemyName)
        {
            case "warrok":
                return warroktDeathCount;
            case "mutant":
                return mutantDeathCount;
            case "vampire":
                return vampireDeathCount;
            default:
                return 0;
        }
    }

    private void SetEnemyHealth(int waveCount)
    {
        UpdateEnemySettings(waveCount, settings => settings.health += _healthGrowth);
    }

    private void SetEnemyDamage(int waveCount)
    {
        UpdateEnemySettings(waveCount,settings=>settings.damage +=_damageGrowth);
    }

    private void UpdateEnemySettings(int waveCount,Action<EnemySettings> update)
    {
        if (waveCount % 5 == 0 && waveCount <= 20)
        {
            update(_warrok.settings);
            update(_vampire.settings);
            update(_mutant.settings);
            if (waveCount == 20)
            {
                SaveEnemySettings();
            }
        }
    }
    private void SaveEnemySettings()
    {
        DataScriptableObject.Save(_mutant.settings, _mutant.settings.enemyName);
        DataScriptableObject.Save(_vampire.settings, _vampire.settings.enemyName);
        DataScriptableObject.Save(_warrok.settings, _warrok.settings.enemyName);
    }
}
