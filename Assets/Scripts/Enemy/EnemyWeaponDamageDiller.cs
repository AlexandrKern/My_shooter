using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponDamageDiller : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        IPlayerDamagiable damageable = other.GetComponent<IPlayerDamagiable>();
        if (damageable != null)
        {
            damageable.TakeDamage(enemy.settings.damage);
        }
    }
}
