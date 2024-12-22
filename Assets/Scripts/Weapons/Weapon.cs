using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public int currrentCountBullet;

    [BoxGroup("Weapon properties")]
    public string weaponName;
    [BoxGroup("Weapon properties")]
    public TypeOfWepon typeOfWepon;
    [BoxGroup("Weapon properties")]
    public GameObject weaponPrefub;
    [BoxGroup("Weapon properties")]
    public TyoeOfShooting tyoeOfShooting;
    [BoxGroup("Weapon properties")]
    public TypeOfBullet[] typeOfSuitableBullets;

    [BoxGroup("Recharge time properties")]
    public float howMuchUpgradeRechargeTime = 10;
    [BoxGroup("Recharge time properties")]
    public int countOfUpgradesRechargeTime = 10;
    [BoxGroup("Recharge time properties")]
    public int priceRechargeTime = 100;
    [BoxGroup("Recharge time properties")]
    public float rechargeTime = 2;
    [BoxGroup("Recharge time properties")]
    public float minRchargeTime = 0.1f;

    [BoxGroup("FireRate properties")]
    public float howMuchUpgradeFireRate = 10;
    [BoxGroup("FireRate properties")]
    public int countOfUpgradesFireRate = 10;
    [BoxGroup("FireRate properties")]
    public int priceFireRate = 100;
    [BoxGroup("FireRate properties")]
    public float fireRate = 1;
    [BoxGroup("FireRate properties")]
    public float minFireRate = 0.1f;

    [BoxGroup("Magazine properties")]
    public int howMuchUpgradeMagazine = 10;
    [BoxGroup("Magazine properties")]
    public int countOfUpgradesMagazine = 10;
    [BoxGroup("Magazine properties")]
    public int priceMagazine = 100;
    [BoxGroup("Magazine properties")]
    public int  magazine = 10;
    [BoxGroup("Magazine properties")]
    public int  maxCountBulletInMagazine = 100;

    private void OnValidate()
    {
        currrentCountBullet = magazine;
    }
}

public enum TyoeOfShooting
{
    Queue,
    Single
}

public enum TypeOfWepon
{
    Pistol,
    MashineGun,
    GrenadeLauncher
}
