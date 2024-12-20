using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RotationBullet", menuName = "Bullets/RotationBullet")]
public class RotationBullet : BulletBase
{
    [BoxGroup("Rotation speed properties")]
    public int countOfUpgradesRotationSpeed = 10;
    [BoxGroup("Rotation speed properties")]
    public int priceRotationSpeed = 100;
    [BoxGroup("Rotation speed properties")]
    public float howMuchRotationSpeed = 10;
    [BoxGroup("Rotation speed properties")]
    public float rotationSpeed = 10;
    [BoxGroup("Rotation speed properties")]
    public float maxRotationSpeed = 100;

    [BoxGroup("Duration properties")]
    public int countOfUpgradesDuration = 10;
    [BoxGroup("Duration properties")]
    public int priceDuration = 100;
    [BoxGroup("Duration properties")]
    public float howMuchRotationDuration = 3;
    [BoxGroup("Duration properties")]
    public float rotationDuration = 3;
    [BoxGroup("Duration properties")]
    public float maxRotationDuration = 20;

    public override int GetCurrentPrise(TypeUpgradeBullet typeUpgradeBullet)
    {
        switch (typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                return priceSpeed;
            case TypeUpgradeBullet.Damage:
                return priceDamage;
            case TypeUpgradeBullet.RotationSpeed:
                return priceRotationSpeed;
            case TypeUpgradeBullet.RotationDuration:
                return priceDuration;
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
            case TypeUpgradeBullet.RotationSpeed:
                priceRotationSpeed = price;
                break;
            case TypeUpgradeBullet.RotationDuration:
                priceDuration = price;
                break;
        }
    }
}
