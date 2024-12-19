using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] private BulletBase[] _bullets;
    private BulletBase _currentBullet;
    private int _currentBulletIndex = 0;

    public event Action<ButtonController, float> OnBulletUpgrade;

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
    public void UpgradeBulletSpeed(ButtonController buttonController)
    {
        if(CheckMoney(_currentBullet.priceSpeed))
        {
            _currentBullet.speed = Mathf.Clamp(_currentBullet.speed + 1, 0, _currentBullet.maxSpeed);
            DataPlayer.SubstractMoney(_currentBullet.priceSpeed);
            _currentBullet.priceSpeed *= 2;
            OnBulletUpgrade?.Invoke(buttonController, _currentBullet.priceSpeed);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletDamage(ButtonController buttonController)
    {
        if (CheckMoney(_currentBullet.priceDamage))
        {
            _currentBullet.damage = Mathf.Clamp(_currentBullet.damage + 1, 0, _currentBullet.maxDamage);
            DataPlayer.SubstractMoney(_currentBullet.priceDamage);
            _currentBullet.priceDamage *= 2;
            OnBulletUpgrade?.Invoke(buttonController, _currentBullet.priceDamage);
            SaveUpgrade();
        }
    }
    public void UpgradeBulletExplosionRadius(ButtonController buttonController)
    {
        if(_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceRadius))
            {
                bullet.explosionRadius = Mathf.Clamp(bullet.explosionRadius + 1, 0, bullet.maxExplosionRadius);
                DataPlayer.SubstractMoney(bullet.priceRadius);
                bullet.priceRadius *= 2;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceRadius);
                SaveUpgrade();
            }
            
        }
    }
    public void UpgradeBulletExplosionForce(ButtonController buttonController)
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceForce))
            {
                bullet.explosionForce = Mathf.Clamp(bullet.explosionForce + 1, 0, bullet.maxExplosionForce);
                DataPlayer.SubstractMoney(bullet.priceForce);
                bullet.priceForce *= 2;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceForce);
                SaveUpgrade();
            }
        }
    }
    public void UpgradeBulletExplosionTimer(ButtonController buttonController)
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceTimer))
            {
                bullet.explosionTimer = Mathf.Clamp(bullet.explosionTimer - 0.1f, bullet.minExplosionTimer, float.MaxValue);
                DataPlayer.SubstractMoney(bullet.priceTimer);
                bullet.priceTimer *= 2;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceTimer);
                SaveUpgrade();
            }
        }
    }
    public void UpgradeBulletRotationSpeed(ButtonController buttonController)
    {
        if (_currentBullet is RotationBullet bullet)
        {
            if (CheckMoney(bullet.priceRotationSpeed))
            {
                bullet.rotationSpeed = Mathf.Clamp(bullet.rotationSpeed + 1, 0, bullet.maxRotationSpeed);
                DataPlayer.SubstractMoney(bullet.priceRotationSpeed);
                bullet.priceRotationSpeed *= 2;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceRotationSpeed);
                SaveUpgrade();
            }
        }
    }
    public void UpgradeBulletRotationDuration(ButtonController buttonController)
    {
        if (_currentBullet is RotationBullet bullet)
        {
            if (CheckMoney(bullet.priceDuration))
            {
                bullet.rotationDuration = Mathf.Clamp(bullet.rotationDuration + 1, 0, bullet.maxRotationDuration);
                DataPlayer.SubstractMoney(bullet.priceDuration);
                bullet.priceDuration *= 2;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceDuration);
                SaveUpgrade();
            }
        }
    }
    private void SaveUpgrade()
    {
        DataScriptableObject.Save(_currentBullet, _currentBullet.bulletName);
    }
    private bool CheckMoney(int amount)
    {
        return amount <= DataPlayer.GetMoney();
    }
    #endregion
}
