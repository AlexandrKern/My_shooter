using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyDamageable
{
    public float deathTimer;
    [HideInInspector] public float health;
    public Action<bool> OnDie;
    private EnemyBase _enemyBase;

    private bool isDeath = false;


    private void Start()
    {
        _enemyBase = GetComponent<EnemyBase>();
        health = _enemyBase.settings.health;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0&&!isDeath) 
        {
            Die();
            isDeath = true;
        }
        
    }

    private void Die()
    {
        DataPlayer.AddEnemyDeathCount(_enemyBase);
        EnemyManager.Instace.AddEnemyDeathCurrentCount(_enemyBase);
        AudioManager.Instance.PlaySFX("EnemyDeath");
        OnDie.Invoke(true);
        Destroy(gameObject,deathTimer);
    }
}
