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
        _input = PlayerControleer.Instace.inputManager._currentInputShoot;
        _playerTransform = PlayerControleer.Instace.playerTransform;
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerControleer.Instace.weponManager.UpgadeWeaponFireRate();
            PlayerControleer.Instace.weponManager.UpgadeWeaponmMagazine();
            PlayerControleer.Instace.weponManager.UpgradeWeaponRechargeTime();
            Debug.Log("Заапргрейдился");
        }
    }

    private void Shoot(bool IsShoot)
    {
        if (_maxCountBulletInMagazine <= 0)
        {
            _isRecharge = true;
            return;
        }
        Weapon weapon = PlayerControleer.Instace.weponManager.GetWeapon();
        if (IsShoot && !_isRecharge && Time.time >= _lastShootTime + weapon.fireRate)
        {
            BulletBase bulletBase = PlayerControleer.Instace.bulletManager.GetBullet();
            foreach (TypeOfBullet type in weapon.TypeOfSuitableBullets)
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
            PlayerControleer.Instace.weponManager.NextWepon();
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
            PlayerControleer.Instace.bulletManager.NextBullet();
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
        _maxCountBulletInMagazine = PlayerControleer.Instace.weponManager.GetWeapon().magazine;
    }
    /// <summary>
    /// Обнуление времени перезарядки
    /// </summary>
    private void SetRechargeTime()
    {
        _rechargeTime = PlayerControleer.Instace.weponManager.GetWeapon().rechargeTime;
    }
    /// <summary>
    /// Устанавлевает оставшиеся патроны
    /// </summary>
    private void SetCurrentCountBullet()
    {
        PlayerControleer.Instace.weponManager.GetWeapon().currrentCountBullet = _maxCountBulletInMagazine;
    }
    /// <summary>
    /// Устанавливает оставшиеся патроны в магазин
    /// </summary>
    private void SetCurrentCountBulletInMagazine()
    {
        _maxCountBulletInMagazine = PlayerControleer.Instace.weponManager.GetWeapon().currrentCountBullet;
    }

    private void SetLustShootTime()
    {
        _lastShootTime = -PlayerControleer.Instace.weponManager.GetWeapon().fireRate;
    }
    private void Recharge(bool isRecharge)
    {
        if (isRecharge && _maxCountBulletInMagazine != PlayerControleer.Instace.weponManager.GetWeapon().magazine)
        {
            _isRecharge = isRecharge;
        }
    }

    private void DetermineTypeOfShooting()
    {
        switch (PlayerControleer.Instace.weponManager.GetWeapon().tyoeOfShooting)
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
