using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerDamagiable
{
    [SerializeField] private int health = 100;
    private int _currentHealth;
    public int Health => _currentHealth;
    public static  Action<int> OnHealthChanged;
    public static Action<bool> OnDie;
    private void Start()
    {
        _currentHealth = health;
    }
    public void TakeDamage(int damage)
    {
       _currentHealth -= damage;
        OnHealthChanged?.Invoke(_currentHealth);
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnDie?.Invoke(true);
        }
    }

    public void  AddHealth(int health)
    {
        _currentHealth += health;
        if(_currentHealth >= this.health)
        {
            _currentHealth = this.health;
        }
        OnHealthChanged?.Invoke(_currentHealth);
    }
}
