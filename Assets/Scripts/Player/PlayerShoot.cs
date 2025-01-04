using System;
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
    private int _maxCountBulletInMagazine;
    private float _lastShootTime;
    public Action<string> OnEndedBullet;
    public Action<Sprite> OnNextWeapon;
    public Action<int,int> OnChangeBulletCount;

    private void Start()
    {
        _input = GameManager.Instace.inputManager._currentInputShoot;
        _playerTransform = PlayerController.Instace.playerTransform;
        bulletBase = GameManager.Instace.bulletManager.GetBullet();
        _currentWeapon = GameManager.Instace.weponManager.GetWeapon();
        _maxCountBulletInMagazine = GameManager.Instace.weponManager.GetWeapon().magazine;
        SetMaxCountBullet();
        SetCurrentCountBullet();
        SetRechargeTime();
        SetLustShootTime();
       
    }

    private void Update()
    {
        DetermineTypeOfShooting();
        NextWepon(_input.IsNextWepon());
        NextBullet(_input.IsNextBullet());
        Recharge(_input.IsRecharge());
        UpdateRechargeTime();
    }

    private void Shoot(bool IsShoot)
    {

        if (_maxCountBulletInMagazine <= 0)
        {
            if(bulletBase.countBullet<=0)OnEndedBullet?.Invoke("��� ��������");
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
                    GameObject bulletPrefub = Instantiate(bulletBase.bulletPrefub
                            , _currentWeapon.firePoint.position
                            , Quaternion.identity);
                    Rigidbody _rb = bulletPrefub.GetComponent<Rigidbody>();
                    _rb.AddForce(bulletDirection * bulletBase.speed, ForceMode.Impulse);
                    _maxCountBulletInMagazine--;
                    OnChangeBulletCount?.Invoke(_maxCountBulletInMagazine,bulletBase.countBullet);
                    _lastShootTime = Time.time;
                    return;
                }
            }
            NextBullet(true);
        }
    }
    private void NextWepon(bool isNextWeapon)
    {
        if(isNextWeapon)
        {
            SetCurrentCountBullet();
            GameManager.Instace.weponManager.NextWepon();
            PlayerController.Instace.ChangeWeapon();
            _currentWeapon = GameManager.Instace.weponManager.GetWeapon();
            OnNextWeapon?.Invoke(_currentWeapon.iconWeapon);

            EnsureCompatibleBullet(); // �������� � ����� ������������ �������

            SetCurrentCountBulletInMagazine();
            OnChangeBulletCount?.Invoke(_currentWeapon.currrentCountBullet, bulletBase.countBullet);
            SetRechargeTime();
            _isRecharge = false;
        }
    }

    /// <summary>
    /// ���������, ��������� �� ������� ������ � ����� �������, � �������� ���������� ��� �������������.
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
        OnChangeBulletCount?.Invoke(_maxCountBulletInMagazine, bulletBase.countBullet);
    }
    private void NextBullet(bool isNextBullet)
    {
        if (isNextBullet)
        {
            bulletBase.countBullet += _maxCountBulletInMagazine;
            do
            {
                GameManager.Instace.bulletManager.NextBullet();
                bulletBase = GameManager.Instace.bulletManager.GetBullet();
            }
            while (!IsBulletCompatibleWithWeapon(bulletBase));
            _maxCountBulletInMagazine = 0;
            _isRecharge = true;
            OnChangeBulletCount?.Invoke(_maxCountBulletInMagazine, bulletBase.countBullet);
        }
    }
    /// <summary>
    /// ���������, ��������� �� ������ � ������� �������
    /// </summary>
    private bool IsBulletCompatibleWithWeapon(BulletBase bullet)
    {
        foreach (TypeOfBullet type in _currentWeapon.typeOfSuitableBullets)
        {
            if (bullet.typeOfBullet == type)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateRechargeTime()
    {
        if (_isRecharge)
        {
            if (_maxCountBulletInMagazine == _currentWeapon.magazine && bulletBase.countBullet == 0) return;
            if (_rechargeTime >= 0)
            {
                Debug.Log("��������������");
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
    /// ��������� ������������� ���������� ��������
    /// </summary>
    private void SetMaxCountBullet()
    {
        if (bulletBase.countBullet == 0) return;

        int weaponMagazine = GameManager.Instace.weponManager.GetWeapon().magazine;
        int remainingSpaceInMagazine = weaponMagazine - _maxCountBulletInMagazine;

        // ���� � ���� ���������� �������� ��� ���������� ��������
        if (bulletBase.countBullet >= remainingSpaceInMagazine)
        {
            _maxCountBulletInMagazine += remainingSpaceInMagazine;
            bulletBase.countBullet -= remainingSpaceInMagazine;
        }
        else
        {
            // ���� �������� ������, ��� ��������� ��� ���������� ��������
            _maxCountBulletInMagazine += bulletBase.countBullet;
            bulletBase.countBullet = 0;
        }

        // �������� ������� � ������������ ����������
        OnChangeBulletCount?.Invoke(_maxCountBulletInMagazine, bulletBase.countBullet);
    }
    /// <summary>
    /// ��������� ������� �����������
    /// </summary>
    private void SetRechargeTime()
    {
        _rechargeTime = GameManager.Instace.weponManager.GetWeapon().rechargeTime;
    }
    /// <summary>
    /// ������������� ���������� �������
    /// </summary>
    private void SetCurrentCountBullet()
    {
        GameManager.Instace.weponManager.GetWeapon().currrentCountBullet = _maxCountBulletInMagazine;
    }
    /// <summary>
    /// ������������� ���������� ������� � �������
    /// </summary>
    private void SetCurrentCountBulletInMagazine()
    {
        _maxCountBulletInMagazine = GameManager.Instace.weponManager.GetWeapon().currrentCountBullet;
    }
    private void SetLustShootTime()
    {
        _lastShootTime = -GameManager.Instace.weponManager.GetWeapon().fireRate;
    }
    private void Recharge(bool isRecharge)
    {
        if (isRecharge && _maxCountBulletInMagazine != GameManager.Instace.weponManager.GetWeapon().magazine)
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
}
