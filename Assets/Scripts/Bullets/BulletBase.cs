using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
public class BulletBase : ScriptableObject
{
    [BoxGroup("Bullet properties")]
    public string bulletName;
    [BoxGroup("Bullet properties")]
    public GameObject bulletPrefub;
    [BoxGroup("Bullet properties")]
    public TypeOfBullet typeOfBullet;
    [BoxGroup("Bullet properties")]
    public List<TypeUpgradeBullet> SupportedUpgrades = new List<TypeUpgradeBullet>();

    [BoxGroup("Speed properties")]
    public int countOfUpgradesSpeed = 10;
    [BoxGroup("Speed properties")]
    public float speed = 10;
    [BoxGroup("Speed properties")]
    public float howMuchUpgradeSpeed = 10;
    [BoxGroup("Speed properties")]
    public float maxSpeed = 100;
    [BoxGroup("Speed properties")]
    public int priceSpeed = 10;

    [BoxGroup("Damage properties")]
    public int countOfUpgradesDamage = 10;
    [BoxGroup("Damage properties")]
    public int damage = 1;
    [BoxGroup("Damage properties")]
    public int howMuchUpgradeDamage = 10;
    [BoxGroup("Damage properties")]
    public int maxDamage = 100;
    [BoxGroup("Damage properties")]
    public int priceDamage = 10;
}

public enum TypeOfBullet
{
    Ordinary,
    Explosion,
    Rotation
}
