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

    public event Action<int> OnAddEnemy;
    public event Action<int> OnRemoveEnemy;

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

}
