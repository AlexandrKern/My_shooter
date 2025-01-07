using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mutant : EnemyBase
{
    [SerializeField] private EnemySettings _settings;
    private NavMeshAgent _agent;
    private Transform _enemyTransform;
    private Transform _playerTransform;
    private EnemyHealth _health;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        _enemyTransform = transform;
        _playerTransform = PlayerController.Instace.playerTransform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _settings.moveSpeed;
    }
    private void OnEnable()
    {
        _health.OnDie += Die;
    }
    private void OnDisable()
    {
        _health.OnDie -= Die;
    }
    public override void Attack()
    {

        if (PlayerDetected()&&canAttack)
        {
            canAttack = false;
            Debug.Log("Враг атаковал");
            OnAttack?.Invoke(false);
            StartCoroutine(AttackCooldown());

        }
    }
    private IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(_settings.attackSpeed);
        OnAttack?.Invoke(true);
        canAttack = true; 
       
    }

    public override void Move()
    {
        if(!PlayerDetected())
        {
            _agent.isStopped = false;
            _agent.SetDestination(_playerTransform.position);
        }
        else
        {
            _agent.isStopped = true;
        }
        OnMove?.Invoke(!_agent.isStopped);

    }

    public override bool PlayerDetected()
    {
        float distanceTiplayer = Vector3.Distance(_enemyTransform.position, _playerTransform.position);
        return distanceTiplayer <=_settings.attackRange;
    }

    private void OnDestroy()
    {
        EnemyManager.Instace.RemoveEnemy(this);
    }

    public override void Die(bool isDeath)
    {
       this.isDeath = !isDeath;
    }
}
