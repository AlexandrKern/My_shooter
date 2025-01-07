using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float health;
    public Action<bool> OnDie;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
        Debug.Log("Враг получил урон");
    }

    private void Die()
    {
        OnDie.Invoke(true);
        Destroy(gameObject,4);
    }
}
