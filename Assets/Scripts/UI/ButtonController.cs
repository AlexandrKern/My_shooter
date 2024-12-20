using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public ButtonType buttonType;

    [ShowIf("IsButtonTextValid")]
    public Text buttonText;

    [ShowIf("buttonType",ButtonType.UpgraddeBullets)]
    public BulletBase bullet;
    [ShowIf("IsBulletUpgradeValid")]
    public TypeUpgradeBullet typeUpgradeBullet;

    [ShowIf("buttonType", ButtonType.UpgradeWeapons)]
    public Weapon weapon;
    [ShowIf("buttonType", ButtonType.UpgradeWeapons)]
    public TypeUpgradeWeapon typeWeaponUpgrade;



    private void Start()
    {
        InitializeUpgradeButtonText();
        GameControler.Instace.weponManager.OnWeaponUpgrade += (buttonController, money) =>
        {
            if(buttonController == this)
            {
                UpdateButtonText(money);
            }
        };
        GameControler.Instace.bulletManager.OnBulletUpgrade += (buttonController, money) =>
        {
            if (buttonController == this)
            {
                UpdateButtonText(money);
            }
        };
    }

    public void ButtonClicked() => GameManager.Instance.ButtonPress(this);
  
    #region Initialize button text

    private void InitializeUpgradeButtonText()
    {
        switch (buttonType)
        {
            case ButtonType.UpgraddeBullets:
                InitializeUpgradeBulletButtonText();
                break;
            case ButtonType.UpgradeWeapons:
                InitializeUpgradeWeponButtonText();
                break;
        }
    }
    private void InitializeUpgradeBulletButtonText()
    {
        if (bullet == null) return;

        Func<BulletBase, int> priceSelector = typeUpgradeBullet switch
        {
            TypeUpgradeBullet.Speed => b => b.priceSpeed,
            TypeUpgradeBullet.Damage => b => b.priceDamage,
            TypeUpgradeBullet.ExplosionRadius => b => (b as ExplosionBullet)?.priceRadius ?? 0,
            TypeUpgradeBullet.ExplosionForce => b => (b as ExplosionBullet)?.priceForce ?? 0,
            TypeUpgradeBullet.ExplosionTimer => b => (b as ExplosionBullet)?.priceTimer ?? 0,
            TypeUpgradeBullet.RotationSpeed => b => (b as RotationBullet)?.priceRotationSpeed ?? 0,
            TypeUpgradeBullet.RotationDuration => b => (b as RotationBullet)?.priceDuration ?? 0,
            _ => null
        };

        if (priceSelector != null)
        {
            int price = priceSelector(bullet);
            UpdateButtonText(price);
        }
    }

    private void InitializeUpgradeWeponButtonText()
    {
        if (buttonType == ButtonType.UpgradeWeapons)
        {
            switch (typeWeaponUpgrade)
            {
                case TypeUpgradeWeapon.RechargeTime:
                    UpdateButtonText(weapon.priceRechargeTime);
                    break;
                case TypeUpgradeWeapon.FireRate:
                    UpdateButtonText(weapon.priceFireRate);
                    break;
                case TypeUpgradeWeapon.Magazine:
                    UpdateButtonText(weapon.priceMagazine);
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateButtonText(int money) => buttonText.text = IsMaxUpgrade() ? "Max" : money.ToString();
    private bool IsMaxUpgrade()
    {
        switch (buttonType)
        {
            case ButtonType.UpgraddeBullets:
                if(typeUpgradeBullet == TypeUpgradeBullet.Speed)
                    return bullet.speed >= bullet.maxSpeed;
                if (typeUpgradeBullet == TypeUpgradeBullet.Damage)
                    return bullet.damage >= bullet.maxDamage;
                if (typeUpgradeBullet == TypeUpgradeBullet.ExplosionForce)
                    if(bullet is ExplosionBullet explosionBullet)
                        return explosionBullet.explosionForce >= explosionBullet.maxExplosionForce;
                if (typeUpgradeBullet == TypeUpgradeBullet.ExplosionRadius)
                    if (bullet is ExplosionBullet explosionBullet)
                        return explosionBullet.explosionRadius >= explosionBullet.maxExplosionRadius;
                if (typeUpgradeBullet == TypeUpgradeBullet.ExplosionTimer)
                    if (bullet is ExplosionBullet explosionBullet)
                        return explosionBullet.explosionTimer <= explosionBullet.minExplosionTimer;
                if (typeUpgradeBullet == TypeUpgradeBullet.RotationSpeed)
                    if (bullet is RotationBullet rotationBullet)
                        return rotationBullet.rotationSpeed >= rotationBullet.maxRotationSpeed;
                if (typeUpgradeBullet == TypeUpgradeBullet.RotationDuration)
                    if (bullet is RotationBullet rotationBullet)
                        return rotationBullet.rotationDuration >= rotationBullet.maxRotationDuration;
                return false;
            case ButtonType.UpgradeWeapons:
                if (typeWeaponUpgrade == TypeUpgradeWeapon.Magazine)
                    return weapon.magazine >= weapon.maxCountBulletInMagazine;
                if (typeWeaponUpgrade == TypeUpgradeWeapon.FireRate)
                    return weapon.fireRate <= weapon.minFireRate;
                if (typeWeaponUpgrade == TypeUpgradeWeapon.RechargeTime)
                    return weapon.rechargeTime <= weapon.minRchargeTime;
                return false;
            default:
                return false;
        }
    
    }
    #endregion

    #region Display inspector
    private void OnValidate()
    {
        if (buttonType == ButtonType.UpgraddeBullets && bullet != null)
        {
            if (!IsUpgradeValidForBullet(typeUpgradeBullet, bullet))
            {
                Debug.LogWarning($"Тип улучшения '{typeUpgradeBullet}' недоступен для пули '{bullet.name}'. Выбери другой тип.");
                typeUpgradeBullet = default; 
            }
        }
    }

    private bool IsBulletUpgradeValid()
    {
        return buttonType == ButtonType.UpgraddeBullets && bullet != null && IsUpgradeValidForBullet(typeUpgradeBullet, bullet);
    }

    private bool IsUpgradeValidForBullet(TypeUpgradeBullet upgradeType, BulletBase bullet)
    {
        return bullet.SupportedUpgrades.Contains(upgradeType);
    }

    private bool IsButtonTextValid()
    {
        return buttonType == ButtonType.UpgraddeBullets||buttonType == ButtonType.UpgradeWeapons;
    }

    #endregion
}
public enum ButtonType
{
    Pause,
    Start,
    Audio,
    UpgraddeBullets,
    UpgradeWeapons
}

public enum TypeUpgradeWeapon
{
    RechargeTime,
    FireRate,
    Magazine
}

public enum TypeUpgradeBullet
{
    Speed,
    Damage,
    ExplosionRadius,
    ExplosionForce,
    ExplosionTimer,
    RotationSpeed,
    RotationDuration
}
