using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyDamageable
{
    public float deathTimer;
    public float health;
    public Action<bool> OnDie;
    private EnemyBase _enemyBase;


    private void Start()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        DataPlayer.AddEnemyDeathCount(_enemyBase);
        EnemyManager.Instace.AddEnemyDeathCurrentCount(_enemyBase);
        OnDie.Invoke(true);
        Destroy(gameObject,deathTimer);
    }
}
