using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletBase _bulletBase;
    private bool _isExplosionTriggered = false;
    private IDamageable _damageable;

    private void Awake()
    {
        _bulletBase = PlayerControleer.Instace.bulletManager.GetBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleBulletType(collision);
    }

    private void HandleBulletType(Collision collision)
    {
        switch (_bulletBase)
        {
            case ExplosionBullet explosionBullet:
                StartCoroutine(HandleExplosionBullet(explosionBullet));
                break;

            case RotationBullet rotationBullet:
                StartCoroutine(HandleRotationBullet(collision.gameObject, rotationBullet.rotationSpeed,rotationBullet.rotationDuration));
                break;

            case OrdinaryBullet ordinaryBullet:
                HandleOrdinaryBullet(collision.gameObject, ordinaryBullet);
                break;

            default:
                Debug.LogWarning("Пуля не найдена");
                break;
        }
    }

    private IEnumerator HandleExplosionBullet(ExplosionBullet explosionBullet)
    {
        float explosionTimer = explosionBullet.explosionTimer;
        _isExplosionTriggered = false;

        while (explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
            yield return null;
        }

        _isExplosionTriggered = true;

        if (_isExplosionTriggered)
        {
            Debug.Log("Взрыв пули");

            Collider[] enemyColiders = Physics.OverlapSphere(transform.position, explosionBullet.explosionRadius, explosionBullet.damageableLayers);

            foreach (Collider enemy in enemyColiders)
            {
                if (IsDamageable(enemy.gameObject))
                {
                    _damageable.TakeDamage(explosionBullet.damage);
                }
                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.AddExplosionForce(explosionBullet.explosionForce,transform.position, explosionBullet.explosionRadius);
                }
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator HandleRotationBullet(GameObject target, float rotationSpeed, float rotationDuration)
    {
        
        if (IsDamageable(target))
        {
            Debug.Log("Объект крутиться");
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                target.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
        
    }

    private void HandleOrdinaryBullet(GameObject target, OrdinaryBullet ordinaryBullet)
    {
        if (IsDamageable(target))
        {
            _damageable.TakeDamage(ordinaryBullet.damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    private bool IsDamageable(GameObject obj)
    {
        _damageable = obj.GetComponent<IDamageable>();
        return _damageable != null;
    }

}

