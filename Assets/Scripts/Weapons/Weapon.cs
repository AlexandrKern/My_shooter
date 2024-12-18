using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [HideInInspector] public Transform firePoint;

    [HideInInspector] public int currrentCountBullet;

    public string weaponName;

    public GameObject weaponPrefub;

    public float rechargeTime = 2;
    public float minRchargeTime = 0.1f;

    public float fireRate = 1;
    public float minFireRate = 0.1f;

    public int  magazine = 10;
    public int  maxCountBulletInMagazine = 100;

    public TyoeOfShooting tyoeOfShooting;
    public TypeOfBullet[] TypeOfSuitableBullets;

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
