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

    private void Update()
    {
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
    } 
    public void AddEnemy(EnemyBase enemy)
    {
        _enemies.Remove(enemy);
    }

}
