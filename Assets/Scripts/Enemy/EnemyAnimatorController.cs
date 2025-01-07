using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private EnemyBase _enemyBase;
    private EnemyHealth _enemyHealth;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyBase = GetComponent<EnemyBase>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        _enemyHealth.OnDie += Die;
        _enemyBase.OnMove += Move;
        _enemyBase.OnAttack += Attack;
    }

    private void OnDisable()
    {
        _enemyHealth.OnDie += Die;
        _enemyBase.OnMove -= Move;
        _enemyBase.OnAttack -= Attack;
    }

    private void Move(bool isMove)
    {
        _animator.SetBool("IsMove", isMove);
    }

    private void Attack(bool isAttack)
    {
        _animator.SetBool("IsAttack", isAttack);
    }

    private void Die(bool isDie)
    {
        _animator.SetBool("IsDie", isDie);
    }
}
