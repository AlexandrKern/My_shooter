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
[RequireComponent(typeof(MoneySpawner))]

public abstract class EnemyBase : MonoBehaviour
{
    public EnemySettings settings;
    protected NavMeshAgent _agent;
    protected Transform _enemyTransform;
    protected Transform _playerTransform;
    protected EnemyHealth _health;

    protected bool canAttack;
    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isMove = true;

    public Action<bool> OnAttack;
    public Action<bool> OnMove;

    protected virtual void Awake()
    {
        _health = GetComponent<EnemyHealth>();
        _enemyTransform = transform;
        _playerTransform = PlayerController.Instace.playerTransform;
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        canAttack = true;
        isDeath = true;
        _agent.speed = settings.moveSpeed;
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
        yield return new WaitForSeconds(settings.attackSpeed);
        OnAttack?.Invoke(true);
        canAttack = true;
    }

    public virtual void Move()
    {
        //Debug.Log("стоит" + _agent.isStopped);
        //Debug.Log("вращаетс€" + _agent.updateRotation);
       
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
        direction.y = 0; // »гнорируем вертикальную ось

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
        return distanceToPlayer <= settings.attackRange;
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
