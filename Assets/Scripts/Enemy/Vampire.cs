using UnityEngine;

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
            StartCoroutine(AttackCooldown(null));
        }
        FaceTarget();
    }

    public void Shoot()
    {
        AudioManager.Instance.PlaySFX("VampireAttack");
        GameObject bullet = Instantiate(_bullet._bulletPrefub, _firePoint.position, Quaternion.identity);
        Vector3 direction = (_playerTransform.position - _firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * _bullet.speed;
    }
}
