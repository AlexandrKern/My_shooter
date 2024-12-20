using NaughtyAttributes;
using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "ExplosionBullet", menuName = "Bullets/ExplosionBullet")]
public class ExplosionBullet : BulletBase
{
    [BoxGroup("Bullet properties")]
    public LayerMask damageableLayers;

    [BoxGroup("Radius properties")]
    public int countOfUpgradesRadius = 10;
    [BoxGroup("Radius properties")]
    public int priceRadius = 100;
    [BoxGroup("Radius properties")]
    public int howMuchExplosionRadius = 10;
    [BoxGroup("Radius properties")]
    public float explosionRadius = 10;
    [BoxGroup("Radius properties")]
    public float maxExplosionRadius = 100;

    [BoxGroup("Force properties")]
    public int countOfUpgradesForce = 10;
    [BoxGroup("Force properties")]
    public int priceForce = 100;
    [BoxGroup("Force properties")]
    public float howMuchExplosionForce = 10;
    [BoxGroup("Force properties")]
    public float explosionForce = 10;
    [BoxGroup("Force properties")]
    public float maxExplosionForce = 100;

    [BoxGroup("Timer properties")]
    public int countOfUpgradesTimer = 10;
    [BoxGroup("Timer properties")]
    public int priceTimer = 100;
    [BoxGroup("Timer properties")]
    public float howMuchExplosionTimer = 10;
    [BoxGroup("Timer properties")]
    public float explosionTimer = 3;
    [BoxGroup("Timer properties")]
    public float minExplosionTimer = 0.1f;

    public override int GetCurrentPrise(TypeUpgradeBullet typeUpgradeBullet)
    {
        switch (typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                return priceSpeed;
            case TypeUpgradeBullet.Damage:
                return priceDamage;
            case TypeUpgradeBullet.ExplosionRadius:
                return priceRadius;
            case TypeUpgradeBullet.ExplosionForce:
                return priceForce;
            case TypeUpgradeBullet.ExplosionTimer:
                return priceTimer;
            default:
                return 0;
        }
    }
    public override void SetCurrentPrise(TypeUpgradeBullet typeUpgradeBullet, int price)
    {
        switch (typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                priceSpeed = price;
                break;
            case TypeUpgradeBullet.Damage:
                priceDamage = price;
                break;
            case TypeUpgradeBullet.ExplosionRadius:
                priceRadius = price;
                break;
            case TypeUpgradeBullet.ExplosionForce:
                priceForce = price;
                break;
            case TypeUpgradeBullet.ExplosionTimer:
                priceTimer = price;
                break;
        }
    }

}
