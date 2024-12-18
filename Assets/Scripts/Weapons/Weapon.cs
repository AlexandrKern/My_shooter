using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "Weapons/Pistol")]
public class Weapon : ScriptableObject
{
    [HideInInspector] public Transform firePoint;

    [HideInInspector] public int currrentCountBullet;

    public GameObject weaponPrefub;

    public float rechargeTime = 2;

    public float fireRate = 1;

    public int  magazine = 10;

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
