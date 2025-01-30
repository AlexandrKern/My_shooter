using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemys/Enemy")]
public class EnemySettings : ScriptableObject
{
    public string enemyName;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public int damage;
    public int health;
}
