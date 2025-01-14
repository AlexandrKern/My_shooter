using System;
using System.Collections;
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

    [SerializeField]private List<EnemyBase> _enemies = new List<EnemyBase>();
    [HideInInspector] public int enemyCount;
    private int warroktDeathCount;
    private int mutantDeathCount;
    private int vampireDeathCount;

    public event Action<int> OnAddEnemy;
    public event Action<int> OnRemoveEnemy;

    private void Start()
    {
        warroktDeathCount = 0;
        mutantDeathCount = 0;
        vampireDeathCount = 0;
    }

    private void Update()
    {
        if(_enemies.Count <= 0) return;
        foreach (EnemyBase enemy in _enemies)
        {
            if (enemy.isDeath)
            {
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

}
