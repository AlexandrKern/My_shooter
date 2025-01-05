using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] private BulletBase[] _bullets;
    private BulletBase _currentBullet;
    private int _currentBulletIndex = 0;

    public event Action<ButtonController, int> OnBulletUpgrade;
    public event Action<ButtonController, bool,int> OnBulletUnlock;
    public event Action<string> OnCheckMoney;

    private void Awake()
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
    public void UnlockBullet(ButtonController buttonController)
    {
        if (CheckMoney(buttonController.bullet.priceBullet))
        {
            SetBullet(buttonController.bullet);
            _currentBullet.isUnlock = true;
            OnBulletUnlock?.Invoke(buttonController, true,_currentBullet.priceBullet);
            DataPlayer.SubstractMoney(_currentBullet.priceBullet);
            Save();
        }
    }

    public BulletBase[] GetAllBullets()
    {
        return _bullets;
    }

    #region Upgrades

    public void UpgradeBullet(ButtonController buttonController)
    {
        SetBullet(buttonController.bullet);
        switch (buttonController.typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                UpgradeBulletSpeed(buttonController);
                break;
            case TypeUpgradeBullet.Damage:
                UpgradeBulletDamage(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionRadius:
                UpgradeBulletExplosionRadius(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionForce:
                UpgradeBulletExplosionForce(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionTimer:
                UpgradeBulletExplosionTimer(buttonController);
                break;
            case TypeUpgradeBullet.RotationSpeed:
                UpgradeBulletRotationSpeed(buttonController);
                break;
            case TypeUpgradeBullet.RotationDuration:
                UpgradeBulletRotationDuration(buttonController);
                break;
        }
    }
    public void UpgradeBulletSpeed(ButtonController buttonController)
    {
        if (CheckMoney(_currentBullet.priceSpeed))
        {
            _currentBullet.speed = Mathf.Clamp(_currentBullet.speed + _currentBullet.howMuchUpgradeSpeed, 0, _currentBullet.maxSpeed);
            DataPlayer.SubstractMoney(_currentBullet.priceSpeed);
            _currentBullet.priceSpeed *= 2;
            _currentBullet.countOfUpgradesSpeed--;
            OnBulletUpgrade?.Invoke(buttonController, _currentBullet.priceSpeed);

            Save();
        }
    }
    public void UpgradeBulletDamage(ButtonController buttonController)
    {
        if (CheckMoney(_currentBullet.priceDamage))
        {
            _currentBullet.damage = Mathf.Clamp(_currentBullet.damage + _currentBullet.howMuchUpgradeDamage, 0, _currentBullet.maxDamage);
            DataPlayer.SubstractMoney(_currentBullet.priceDamage);
            _currentBullet.priceDamage *= 2;
            _currentBullet.countOfUpgradesDamage--;
            OnBulletUpgrade?.Invoke(buttonController, _currentBullet.priceDamage);
            Save();
        }
    }
    public void UpgradeBulletExplosionRadius(ButtonController buttonController)
    {
        if(_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceRadius))
            {
                bullet.explosionRadius = Mathf.Clamp(bullet.explosionRadius + bullet.howMuchExplosionRadius, 0, bullet.maxExplosionRadius);
                DataPlayer.SubstractMoney(bullet.priceRadius);
                bullet.priceRadius *= 2;
                bullet.countOfUpgradesRadius--;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceRadius);
                Save();
            }
            
        }
    }
    public void UpgradeBulletExplosionForce(ButtonController buttonController)
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceForce))
            {
                bullet.explosionForce = Mathf.Clamp(bullet.explosionForce + bullet.howMuchExplosionForce, 0, bullet.maxExplosionForce);
                DataPlayer.SubstractMoney(bullet.priceForce);
                bullet.priceForce *= 2;
                bullet.countOfUpgradesForce--;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceForce);
                Save();
            }
        }
    }
    public void UpgradeBulletExplosionTimer(ButtonController buttonController)
    {
        if (_currentBullet is ExplosionBullet bullet)
        {
            if (CheckMoney(bullet.priceTimer))
            {
                bullet.explosionTimer = Mathf.Clamp(bullet.explosionTimer - bullet.howMuchExplosionTimer, bullet.minExplosionTimer, float.MaxValue);
                DataPlayer.SubstractMoney(bullet.priceTimer);
                bullet.priceTimer *= 2;
                bullet.countOfUpgradesTimer--;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceTimer);
                Save();
            }
        }
    }
    public void UpgradeBulletRotationSpeed(ButtonController buttonController)
    {
        if (_currentBullet is RotationBullet bullet)
        {
            if (CheckMoney(bullet.priceRotationSpeed))
            {
                bullet.rotationSpeed = Mathf.Clamp(bullet.rotationSpeed + bullet.howMuchRotationSpeed, 0, bullet.maxRotationSpeed);
                DataPlayer.SubstractMoney(bullet.priceRotationSpeed);
                bullet.priceRotationSpeed *= 2;
                bullet.countOfUpgradesRotationSpeed--;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceRotationSpeed);
                Save();
            }
        }
    }
    public void UpgradeBulletRotationDuration(ButtonController buttonController)
    {
        if (_currentBullet is RotationBullet bullet)
        {
            if (CheckMoney(bullet.priceDuration))
            {
                bullet.rotationDuration = Mathf.Clamp(bullet.rotationDuration + bullet.howMuchRotationDuration, 0, bullet.maxRotationDuration);
                DataPlayer.SubstractMoney(bullet.priceDuration);
                bullet.priceDuration *= 2;
                bullet.countOfUpgradesDuration--;
                OnBulletUpgrade?.Invoke(buttonController, bullet.priceDuration);
                Save();
            }
        }
    }
    private void Save()
    {
        DataScriptableObject.Save(_currentBullet, _currentBullet.bulletName);
    }
    private bool CheckMoney(int amount)
    {
        if (amount <= DataPlayer.GetMoney())
        {
            return true;
        }
        else
        {
            OnCheckMoney?.Invoke("Не хватает денег");
            return false;
        }

    }
    #endregion
}
