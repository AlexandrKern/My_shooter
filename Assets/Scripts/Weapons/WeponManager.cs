using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class WeponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    private Weapon[] _newWeapons;
    private List<GameObject> _weaponOjects = new List<GameObject>(); 
    private Weapon _currentWeapon;
    private Transform _weponPosition;
    private int _currentWeaponIndex = 0;

    public event Action<ButtonController, int> OnWeaponUpgrade;

    private void Start()
    {
         InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        _weponPosition = PlayerController.Instace.weaponPosition;
        foreach (Weapon weapon in weapons)
        {
            GameObject newWeapon = Instantiate(weapon.weaponPrefub, _weponPosition.position, Quaternion.identity);
            newWeapon.transform.SetParent(_weponPosition);
            newWeapon.SetActive(false);
            weapon.firePoint = newWeapon.transform.Find("FirePoint"); 
            _weaponOjects.Add(newWeapon);
        }
        _newWeapons = weapons;
        _currentWeapon = _newWeapons[0];
    }

    public Weapon GetWeapon()
    {
        _weaponOjects[_currentWeaponIndex].SetActive(true);
        return _currentWeapon;
    }
    public void SetWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
    }
    public void NextWepon()
    {
        _weaponOjects[_currentWeaponIndex].SetActive(false);
        _currentWeaponIndex = (_currentWeaponIndex - 1 + _newWeapons.Length) % _newWeapons.Length;
        _currentWeapon = _newWeapons[_currentWeaponIndex];
    }

    #region Upgades


    public void UpgradeWepon(ButtonController buttonController)
    {
        SetWeapon(buttonController.weapon);
        switch (buttonController.typeWeaponUpgrade)
        {
            case TypeUpgradeWeapon.RechargeTime:
                UpgradeWeaponRechargeTime(buttonController);
                break;
            case TypeUpgradeWeapon.FireRate:
                UpgadeWeaponFireRate(buttonController);
                break;
            case TypeUpgradeWeapon.Magazine:
                UpgadeWeaponmMagazine(buttonController);
                break;
            default:
                break;
        }
    }

    public void UpgradeWeaponRechargeTime(ButtonController buttonController)
    {
        if(CheckMoney(_currentWeapon.priceRechargeTime))
        {
            _currentWeapon.rechargeTime = Mathf.Clamp(_currentWeapon.rechargeTime - 0.1f, _currentWeapon.minRchargeTime, float.MaxValue);
            DataPlayer.SubstractMoney(_currentWeapon.priceRechargeTime);
            _currentWeapon.priceRechargeTime *= 2;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceRechargeTime);
            SaveUpgrade();
        }
    }
    public void UpgadeWeaponFireRate(ButtonController buttonController)
    {
        if (CheckMoney(_currentWeapon.priceFireRate))
        {
            _currentWeapon.fireRate = Mathf.Clamp(_currentWeapon.fireRate - 0.1f, _currentWeapon.minFireRate, float.MaxValue);
            DataPlayer.SubstractMoney(_currentWeapon.priceFireRate);
            _currentWeapon.priceFireRate *= 2;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceFireRate);
            SaveUpgrade();
        }
    }
    public void UpgadeWeaponmMagazine(ButtonController buttonController)
    {
        if (CheckMoney(_currentWeapon.priceFireRate))
        {
            _currentWeapon.magazine = Mathf.Clamp(_currentWeapon.magazine + 1, 0, _currentWeapon.maxCountBulletInMagazine);
            DataPlayer.SubstractMoney(_currentWeapon.priceMagazine);
            _currentWeapon.priceMagazine *= 2;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceMagazine);
            SaveUpgrade();
        }
    }
    private void SaveUpgrade()
    {
        DataScriptableObject.Save(_currentWeapon, _currentWeapon.weaponName);
    }

    private bool CheckMoney(int amount)
    {
        return amount <= DataPlayer.GetMoney();
    }
    #endregion
}
