using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool canAttack = true;
    public Action<bool> OnMove;
    public Action<bool> OnAttack;
    public abstract void Move();
    public abstract void Attack();
    public abstract void Die(bool isDeath);
    public abstract bool PlayerDetected();
}
