using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class WeponManager : MonoBehaviour
{
    public Weapon[] weapons;
    [HideInInspector] public List<Weapon> unlockWeaponList = new List<Weapon>();
    private Weapon _currentWeapon;
    private int _currentWeaponIndex = 0;

    public event Action<ButtonController, int> OnWeaponUpgrade;
    public event Action<ButtonController, bool> OnWeaponUnlock;

    private void Awake()
    {
         InitializeWeapon();
    }
    private void InitializeWeapon()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.isUnlock)
            {
                unlockWeaponList.Add(weapon);
            }
        }
        _currentWeapon = unlockWeaponList[0];
    }
    public Weapon GetWeapon()
    {
        return _currentWeapon;
    }
    public void SetWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
    }
    public void NextWepon()
    {
        _currentWeaponIndex = (_currentWeaponIndex - 1 + unlockWeaponList.Count) % unlockWeaponList.Count;
        _currentWeapon = unlockWeaponList[_currentWeaponIndex];
    }

    public void UnlockWeapon(ButtonController buttonController)
    {
        if (CheckMoney(buttonController.weapon.priceWeapon))
        {
            SetWeapon(buttonController.weapon);
            _currentWeapon.isUnlock = true;
            OnWeaponUnlock?.Invoke(buttonController, true);
            InitializeWeapon();
            Save();
        }
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
            _currentWeapon.rechargeTime = Mathf.Clamp(_currentWeapon.rechargeTime - _currentWeapon.howMuchUpgradeRechargeTime, _currentWeapon.minRchargeTime, float.MaxValue);
            DataPlayer.SubstractMoney(_currentWeapon.priceRechargeTime);
            _currentWeapon.priceRechargeTime *= 2;
            _currentWeapon.countOfUpgradesRechargeTime--;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceRechargeTime);
            Save();
        }
    }
    public void UpgadeWeaponFireRate(ButtonController buttonController)
    {
        if (CheckMoney(_currentWeapon.priceFireRate))
        {
            _currentWeapon.fireRate = Mathf.Clamp(_currentWeapon.fireRate - _currentWeapon.howMuchUpgradeFireRate, _currentWeapon.minFireRate, float.MaxValue);
            DataPlayer.SubstractMoney(_currentWeapon.priceFireRate);
            _currentWeapon.priceFireRate *= 2;
            _currentWeapon.countOfUpgradesFireRate--;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceFireRate);
            Save();
        }
    }
    public void UpgadeWeaponmMagazine(ButtonController buttonController)
    {
        if (CheckMoney(_currentWeapon.priceFireRate))
        {
            _currentWeapon.magazine = Mathf.Clamp(_currentWeapon.magazine + _currentWeapon.howMuchUpgradeMagazine, 0, _currentWeapon.maxCountBulletInMagazine);
            DataPlayer.SubstractMoney(_currentWeapon.priceMagazine);
            _currentWeapon.priceMagazine *= 2;
            _currentWeapon.countOfUpgradesMagazine--;
            OnWeaponUpgrade?.Invoke(buttonController, _currentWeapon.priceMagazine);
            Save();
        }
    }
    private void Save()
    {
        DataScriptableObject.Save(_currentWeapon, _currentWeapon.weaponName);
    }

    private bool CheckMoney(int amount)
    {
        return amount <= DataPlayer.GetMoney();
    }
    #endregion
}
