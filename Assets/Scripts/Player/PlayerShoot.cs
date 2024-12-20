using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private IInputShooter _input;
    private Transform _playerTransform;
    private float _rechargeTime;
    private bool _isRecharge = false;
    private int _maxCountBulletInMagazine;
    private float _lastShootTime;

    private void Start()
    {
        _input = GameControler.Instace.inputManager._currentInputShoot;
        _playerTransform = PlayerController.Instace.playerTransform;
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
            _isRecharge = true;
            return;
        }
        Weapon weapon = GameControler.Instace.weponManager.GetWeapon();
        if (IsShoot && !_isRecharge && Time.time >= _lastShootTime + weapon.fireRate)
        {
            BulletBase bulletBase = GameControler.Instace.bulletManager.GetBullet();
            foreach (TypeOfBullet type in weapon.typeOfSuitableBullets)
            {
                if (bulletBase.typeOfBullet == type)
                {
                    Vector3 bulletDirection = _playerTransform.forward;
                    GameObject bulletPrefub = Instantiate(bulletBase.bulletPrefub
                            , weapon.firePoint.position
                            , Quaternion.identity);
                    Rigidbody _rb = bulletPrefub.GetComponent<Rigidbody>();
                    _rb.AddForce(bulletDirection * bulletBase.speed, ForceMode.Impulse);
                    _maxCountBulletInMagazine--;
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
            GameControler.Instace.weponManager.NextWepon();
            SetCurrentCountBulletInMagazine();
            SetRechargeTime();
            _isRecharge = false;
        }
    }
    private void NextBullet(bool isNextBullet)
    {
        if (isNextBullet)
        {
            Debug.Log("Нажал кнопку поменять пулю");
            GameControler.Instace.bulletManager.NextBullet();
        }
    }

    private void UpdateRechargeTime()
    {
        if (_isRecharge)
        {
            if (_rechargeTime >= 0)
            {
                Debug.Log("Перезаряжается");
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
        _maxCountBulletInMagazine = GameControler.Instace.weponManager.GetWeapon().magazine;
    }
    /// <summary>
    /// Обнуление времени перезарядки
    /// </summary>
    private void SetRechargeTime()
    {
        _rechargeTime = GameControler.Instace.weponManager.GetWeapon().rechargeTime;
    }
    /// <summary>
    /// Устанавлевает оставшиеся патроны
    /// </summary>
    private void SetCurrentCountBullet()
    {
        GameControler.Instace.weponManager.GetWeapon().currrentCountBullet = _maxCountBulletInMagazine;
    }
    /// <summary>
    /// Устанавливает оставшиеся патроны в магазин
    /// </summary>
    private void SetCurrentCountBulletInMagazine()
    {
        _maxCountBulletInMagazine = GameControler.Instace.weponManager.GetWeapon().currrentCountBullet;
    }

    private void SetLustShootTime()
    {
        _lastShootTime = -GameControler.Instace.weponManager.GetWeapon().fireRate;
    }
    private void Recharge(bool isRecharge)
    {
        if (isRecharge && _maxCountBulletInMagazine != GameControler.Instace.weponManager.GetWeapon().magazine)
        {
            _isRecharge = isRecharge;
        }
    }

    private void DetermineTypeOfShooting()
    {
        switch (GameControler.Instace.weponManager.GetWeapon().tyoeOfShooting)
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
