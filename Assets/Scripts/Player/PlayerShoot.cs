using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    private IInputShooter _input;
    private Transform _playerTransform;
    private Weapon _currentWeapon;
    BulletBase bulletBase;
    private float _rechargeTime;
    private bool _isRecharge = false;
    private int _countBulletInMagazine;
    private float _lastShootTime;
    public Action<string> OnEndedBullet;
    public Action<Sprite> OnNextWeapon;
    public Action<Sprite> OnNextBullet;
    public Action<int> OnChangeBulletCount;
    public Action<int> OnChangeBulletCountInmagazine;
    private bool _isDeath = false;

    private void Start()
    {
        _input = GameManager.Instace.inputManager._currentInputShoot;
        _playerTransform = PlayerController.Instace.playerTransform;
        bulletBase = GameManager.Instace.bulletManager.GetBullet();
        _currentWeapon = GameManager.Instace.weponManager.GetWeapon();
        SetCountBulletInMagazine();
        SetRechargeTime();
        SetLustShootTime();

    }
    private void OnEnable()
    {
        PlayerHealth.OnDie += SetIsDeath;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDie -= SetIsDeath;
    }

    private void Update()
    {
        if (!_isDeath)
        {
            DetermineTypeOfShooting();
            NextWepon(_input.IsNextWepon());
            NextBullet(_input.IsNextBullet());
            Recharge(_input.IsRecharge());
            UpdateRechargeTime();
            if (_input is InputHandheld handheld)
            {
                handheld.ResetFlags();
            }
        }
       
    }

    private void Shoot(bool IsShoot)
    {
        if (_countBulletInMagazine <= 0&& bulletBase.countBullet <= 0&&IsShoot)
        {
                OnEndedBullet?.Invoke("Нет снарядов");
                return;
        }
        if (_countBulletInMagazine <= 0 &&  bulletBase.countBullet != 0)
        {
            _isRecharge = true;
            return;
        }
        if (IsShoot && !_isRecharge && Time.time >= _lastShootTime + _currentWeapon.fireRate)
        {
            bulletBase = GameManager.Instace.bulletManager.GetBullet();
            foreach (TypeOfBullet type in _currentWeapon.typeOfSuitableBullets)
            {
                if (bulletBase.typeOfBullet == type)
                {
                    if (!bulletBase.isUnlock)
                    {
                        NextBullet(true);
                        return;
                    }
                    Vector3 bulletDirection = _currentWeapon.firePoint.forward;
                    bulletDirection = Quaternion.Euler(0, -33, 0) * bulletDirection;
                    GameObject bulletPrefub = Instantiate(bulletBase.bulletPrefub
                            , _currentWeapon.firePoint.position
                            , Quaternion.identity);
                    Rigidbody _rb = bulletPrefub.GetComponent<Rigidbody>();
                    _rb.AddForce(bulletDirection * bulletBase.speed, ForceMode.Impulse);
                    _countBulletInMagazine--;
                    OnChangeBulletCount?.Invoke(bulletBase.countBullet);
                    OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
                    _lastShootTime = Time.time;
                    return;
                }
            }
            NextBullet(true);
        }
    }
    private void NextWepon(bool isNextWeapon)
    {
        if (isNextWeapon)
        {
            SetMagazineByTypeBullet();
            GameManager.Instace.weponManager.NextWepon();
            SetRechargeTime();
            _currentWeapon = GameManager.Instace.weponManager.GetWeapon();
            PlayerController.Instace.ChangeWeapon();
            if (GetAvailableUnlockedCompatibleBulletTypesCount() == 0)
            {
                OnEndedBullet?.Invoke("Нет доступных пуль для нового оружия");
                return;
            }
            EnsureCompatibleBullet();
            SetCountBulletInMagazine();
            OnNextWeapon?.Invoke(_currentWeapon.iconWeapon);
            OnChangeBulletCount?.Invoke(bulletBase.countBullet);
            OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
        }
    }

    /// <summary>
    /// Проверяет, совместим ли текущий снаряд с новым оружием, и выбирает подходящий при необходимости.
    /// </summary>
    private void EnsureCompatibleBullet()
    {
        if (!IsBulletCompatibleWithWeapon(bulletBase))
        {
            do
            {
                GameManager.Instace.bulletManager.NextBullet();
                bulletBase = GameManager.Instace.bulletManager.GetBullet();
            }
            while (!IsBulletCompatibleWithWeapon(bulletBase));
        }
        OnNextBullet?.Invoke(bulletBase.icon);
        OnChangeBulletCount?.Invoke(bulletBase.countBullet);
        OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
    }
    private void NextBullet(bool isNextBullet)
    {
        if (isNextBullet)
        {
            SetMagazineByTypeBullet();
            GameManager.Instace.bulletManager.NextBullet();
            bulletBase = GameManager.Instace.bulletManager.GetBullet();
            EnsureCompatibleBullet();
            SetCountBulletInMagazine();
            OnNextBullet?.Invoke(bulletBase.icon);
            OnChangeBulletCount?.Invoke(bulletBase.countBullet);
            OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
        }
    }
    /// <summary>
    /// Получает количество пуль которые может использовать оружие
    /// </summary>
    /// <returns></returns>
    private int GetAvailableUnlockedCompatibleBulletTypesCount()
    {
        BulletBase[] bullets = GameManager.Instace.bulletManager.GetAllBullets();
        List<TypeOfBullet> typeOfBullets = _currentWeapon.typeOfSuitableBullets.ToList(); ;
        int count = 0;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (_currentWeapon.typeOfSuitableBullets.Contains(bullets[i].typeOfBullet) && bullets[i].isUnlock)
            {
                count++;
            }
        }
        return count;
    }
    /// <summary>
    /// Проверяет, совместим ли снаряд с текущим оружием
    /// </summary>
    private bool IsBulletCompatibleWithWeapon(BulletBase bullet)
    {
        foreach (TypeOfBullet type in _currentWeapon.typeOfSuitableBullets)
        {
            if (bullet.typeOfBullet == type)
            {
                if (bullet.isUnlock)
                {
                    return true;
                }

            }
        }
        return false;
    }

    private void UpdateRechargeTime()
    {
        if (_isRecharge)
        {
            if (_countBulletInMagazine == _currentWeapon.magazine && bulletBase.countBullet == 0) return;

            if (_rechargeTime >= 0)
            {
                _rechargeTime -= Time.deltaTime;
            }
            if (_rechargeTime <= 0)
            {
                _isRecharge = false;
                SetMaxCountBullet();
                SetRechargeTime();
            }
        }

    }

    /// <summary>
    /// Установка максимального количества патронов
    /// </summary>
    private void SetMaxCountBullet()
    {
        if (bulletBase.countBullet == 0) return;

        int weaponMagazine = GameManager.Instace.weponManager.GetWeapon().magazine;
        int remainingSpaceInMagazine = weaponMagazine - _countBulletInMagazine;

        // Если в пуле достаточно патронов для заполнения магазина
        if (bulletBase.countBullet >= remainingSpaceInMagazine)
        {
            _countBulletInMagazine += remainingSpaceInMagazine;
            bulletBase.countBullet -= remainingSpaceInMagazine;
        }
        else
        {
            // Если патронов меньше, чем требуется для заполнения магазина
            _countBulletInMagazine += bulletBase.countBullet;
            bulletBase.countBullet = 0;
        }

        // Вызываем событие с обновленными значениями
        OnChangeBulletCount?.Invoke(bulletBase.countBullet);
        OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
    }
    /// <summary>
    /// Устанавливает патроны в магазин
    /// </summary>
    private void SetCountBulletInMagazine()
    {
        switch (bulletBase.typeOfBullet)
        {
            case TypeOfBullet.Ordinary:
                _countBulletInMagazine = _currentWeapon.countOrdinaryBullet;
                break;
            case TypeOfBullet.Explosion:
                _countBulletInMagazine = _currentWeapon.countExplosionBullet;
                break;
            case TypeOfBullet.Rotation:
                _countBulletInMagazine = _currentWeapon.countRotationBullet;
                break;
            default:
                break;
        }
        OnChangeBulletCount?.Invoke(bulletBase.countBullet);
        OnChangeBulletCountInmagazine?.Invoke(_countBulletInMagazine);
    }
    /// <summary>
    /// Устанавливает конкректные паторны в подходящий магазин
    /// </summary>
    private void SetMagazineByTypeBullet()
    {
        switch (bulletBase.typeOfBullet)
        {
            case TypeOfBullet.Ordinary:
                _currentWeapon.countOrdinaryBullet = _countBulletInMagazine;
                break;
            case TypeOfBullet.Explosion:
                _currentWeapon.countExplosionBullet = _countBulletInMagazine;
                break;
            case TypeOfBullet.Rotation:
                _currentWeapon.countRotationBullet = _countBulletInMagazine;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Обнуление времени перезарядки
    /// </summary>
    private void SetRechargeTime()
    {
        _rechargeTime = GameManager.Instace.weponManager.GetWeapon().rechargeTime;
    }
    private void SetLustShootTime()
    {
        _lastShootTime = -GameManager.Instace.weponManager.GetWeapon().fireRate;
    }
    private void Recharge(bool isRecharge)
    {
        if (isRecharge && _countBulletInMagazine != GameManager.Instace.weponManager.GetWeapon().magazine&&bulletBase.countBullet!=0)
        {
            _isRecharge = isRecharge;
        }
    }
    private void DetermineTypeOfShooting()
    {
        switch (GameManager.Instace.weponManager.GetWeapon().tyoeOfShooting)
        {
            case TyoeOfShooting.Queue:
                Shoot(_input.IsShootQueue());
                break;
            case TyoeOfShooting.Single:
                Shoot(_input.IsShootSingle());
                break;
            default:
                break;
        }
    }
    private void SetIsDeath(bool isDeath)
    {
        _isDeath = isDeath;
    }
}
