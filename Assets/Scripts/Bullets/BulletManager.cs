using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] private BulletBase[] _bullets;
    private BulletBase _currentBullet;
    private int _currentBulletIndex = 0;

    private void Start()
    {
        _currentBullet = _bullets[0];
    }

    public BulletBase GetBullet()
    {
        return _currentBullet;
    }

    public void SetBullet(BulletBase bullet)
    {
        _currentBullet = bullet;
    }

    public void NextBullet()
    {
        _currentBulletIndex = (_currentBulletIndex - 1 +_bullets.Length) % _bullets.Length;
        _currentBullet = _bullets[_currentBulletIndex];
    }

    #region Upgrades

    public void UpgradeBulletSpeed()
    {
        _currentBullet.speed = Mathf.Clamp(_currentBullet.speed + 1, 0, _currentBullet.maxSpeed);
        SaveUpgrade();
    }
    public void UpgradeBulletDamage()
    {
        _currentBullet.damage = Mathf.Clamp(_currentBullet.damage + 1, 0, _currentBullet.maxDamage);
        SaveUpgrade();
    }
    public void UpgradeBulletExplosionRadius()
    {
        if(_currentBullet is ExplosionBullet bullet)
        {
            bullet.explosionRadius = Mathf.Clamp(bullet.explosionRadius + 1, 0, bullet.maxExplosionRadius);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletExplosionForce()
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            bullet.explosionForce = Mathf.Clamp(bullet.explosionForce + 1, 0, bullet.maxExplosionForce);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletExplosionTimer()
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            bullet.explosionTimer = Mathf.Clamp(bullet.explosionTimer - 0.1f, bullet.minExplosionTimer, float.MaxValue);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletRotationSpeed()
    {
        if (_currentBullet is RotationBullet bullet)
        {
            bullet.rotationSpeed = Mathf.Clamp(bullet.rotationSpeed + 1, 0, bullet.maxRotationSpeed);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletRotationDuration()
    {
        if (_currentBullet is RotationBullet bullet)
        {
            bullet.rotationDuration = Mathf.Clamp(bullet.rotationDuration + 1, 0, bullet.rotationDuration);
            SaveUpgrade();
        }
    }
    private void SaveUpgrade()
    {
        Data.Save(_currentBullet, _currentBullet.bulletName);
    }
    #endregion
}
