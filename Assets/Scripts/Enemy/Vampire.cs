using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire : EnemyBase
{
    [SerializeField] private EnemyBullet _bullet;
    [SerializeField] private Transform _firePoint;

    public override void Attack()
    {
        if (PlayerDetected() && canAttack)
        {
            OnAttack?.Invoke(false);
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
        FaceTarget();
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bullet._bulletPrefub, _firePoint.position, Quaternion.identity);
        Vector3 direction = (_playerTransform.position - _firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * _bullet.speed;
    }
}
