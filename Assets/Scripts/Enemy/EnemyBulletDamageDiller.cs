using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDamageDiller : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerDamagiable damageable = collision.gameObject.GetComponent<IPlayerDamagiable>();
        if (damageable != null)
        {
            damageable.TakeDamage(enemy.settings.damage);
            Destroy(gameObject);
        }
    }
}
