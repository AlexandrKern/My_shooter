using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyAnimatorController))]

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemySettings _settings;
    protected NavMeshAgent _agent;
    protected Transform _enemyTransform;
    protected Transform _playerTransform;
    protected EnemyHealth _health;

    protected bool canAttack;
    [HideInInspector] public bool isDeath;

    public Action<bool> OnAttack;
    public Action<bool> OnMove;

    protected virtual void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    protected virtual void Start()
    {
        canAttack = true;
        isDeath = true;
        _enemyTransform = transform;
        _playerTransform = PlayerController.Instace.playerTransform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _settings.moveSpeed;
    }

    protected virtual void OnEnable()
    {
        _health.OnDie += Die;
    }

    protected virtual void OnDisable()
    {
        _health.OnDie -= Die;
    }

    public abstract void Attack();

    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_settings.attackSpeed);
        OnAttack?.Invoke(true);
        canAttack = true;
    }

    public virtual void Move()
    {
        if (!PlayerDetected())
        {
            _agent.isStopped = false;
            _agent.SetDestination(_playerTransform.position);
            OnAttack?.Invoke(false);
        }
        else
        {
            if(canAttack)
            {
                _agent.isStopped = true;
            }
            
        }
        OnMove?.Invoke(!PlayerDetected());
    }

    protected virtual void FaceTarget()
    {
        Vector3 direction = _playerTransform.position - _enemyTransform.position;
        direction.y = 0; // Игнорируем вертикальную ось

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyTransform.rotation = Quaternion.Slerp(
                _enemyTransform.rotation,
                targetRotation,
                Time.deltaTime * _agent.angularSpeed
            );
        }
    }

    public virtual bool PlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);
        return distanceToPlayer <= _settings.attackRange;
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.Instace.RemoveEnemy(this);
    }

    public virtual void Die(bool isDeath)
    {
        this.isDeath = !isDeath;
    }

}
