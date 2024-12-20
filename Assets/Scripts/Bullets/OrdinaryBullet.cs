
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "OrdinaryBullet", menuName = "Bullets/OrdinaryBullet")]
public class OrdinaryBullet : BulletBase
{
    public override int GetCurrentPrise(TypeUpgradeBullet typeUpgradeBullet)
    {
        switch (typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                return priceSpeed;
            case TypeUpgradeBullet.Damage:
                return priceDamage;
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

        }
    }
}
