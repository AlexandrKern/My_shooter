using NaughtyAttributes;
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
        InitializeUpgradeBulletButtonText();
        InitializeUpgradeWeponButtonText();
        PlayerControleer.Instace.weponManager.OnWeaponUpgrade += (buttonController, money) =>
        {
            if(buttonController == this)
            {
                UpdateButtonText(money);
            }
        };

        PlayerControleer.Instace.bulletManager.OnBulletUpgrade += (buttonController, money) =>
        {
            if (buttonController == this)
            {
                UpdateButtonText(money);
            }

        };
    }
    #region Initialize button text
    public void ButtonClicked()
    {
        GameManager.Instance.ButtonPress(buttonType, bullet, typeUpgradeBullet, weapon, typeWeaponUpgrade,this);
    }

    private void InitializeUpgradeBulletButtonText()
    {
        if (buttonType == ButtonType.UpgraddeBullets)
        {
            switch (typeUpgradeBullet)
            {
                case TypeUpgradeBullet.Speed:
                    SetBulletButttonSpeedText(bullet);
                    break;
                case TypeUpgradeBullet.Damage:
                    SetBulletButttonDamageText(bullet);
                    break;
                case TypeUpgradeBullet.ExplosionRadius:
                    SetBulletButttonRadiusText(bullet);
                    break;
                case TypeUpgradeBullet.ExplosionForce:
                    SetBulletButttonForceText(bullet);
                    break;
                case TypeUpgradeBullet.ExplosionTimer:
                    SetBulletButttonTimerText(bullet);
                    break;
                case TypeUpgradeBullet.RotationSpeed:
                    SetBulletButttonRotationSpeedText(bullet);
                    break;
                case TypeUpgradeBullet.RotationDuration:
                    SetBulletButttonDurationText(bullet);
                    break;
                default:
                    break;
            }
        }
       
    }

    private void SetBulletButttonSpeedText(BulletBase bullet)
    {
        switch (bullet)
        {
            case ExplosionBullet explosionBullet:
                UpdateButtonText(explosionBullet.priceSpeed); 
                break;
            case OrdinaryBullet ordinaryBullet:
                UpdateButtonText(ordinaryBullet.priceSpeed);
                break;
            case RotationBullet rotationBullet:
                UpdateButtonText(rotationBullet.priceSpeed);
                break;
            default:
                break;
        }
    }

    private void SetBulletButttonDamageText(BulletBase bullet)
    {
        switch (bullet)
        {
            case ExplosionBullet explosionBullet:
                UpdateButtonText(explosionBullet.priceDamage);
                break;
            case OrdinaryBullet ordinaryBullet:
                UpdateButtonText(ordinaryBullet.priceDamage);
                break;
            case RotationBullet rotationBullet:
                UpdateButtonText(rotationBullet.priceDamage);
                break;
            default:
                break;
        }
    }

    private void SetBulletButttonRadiusText(BulletBase bullet)
    {
        switch (bullet)
        {
            case ExplosionBullet explosionBullet:
                UpdateButtonText(explosionBullet.priceRadius);
                break;
            default:
                break;
        }
    }

    private void SetBulletButttonForceText(BulletBase bullet)
    {
        switch (bullet)
        {
            case ExplosionBullet explosionBullet:
                UpdateButtonText(explosionBullet.priceForce);
                break;
            default:
                break;
        }
    }
    private void SetBulletButttonTimerText(BulletBase bullet)
    {
        switch (bullet)
        {
            case ExplosionBullet explosionBullet:
                UpdateButtonText(explosionBullet.priceTimer);
                break;
            default:
                break;
        }
    }

    private void SetBulletButttonDurationText(BulletBase bullet)
    {
        switch (bullet)
        {
            case RotationBullet rotationBullet:
                UpdateButtonText(rotationBullet.priceDuration);
                break;
            default:
                break;
        }
    }

    private void SetBulletButttonRotationSpeedText(BulletBase bullet)
    {
        switch (bullet)
        {
            case RotationBullet rotationBullet:
                UpdateButtonText(rotationBullet.priceRotationSpeed);
                break;
            default:
                break;
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

    private void UpdateButtonText(float money)
    {
       buttonText.text = money.ToString();
    }
    #endregion

    #region Didplay inspector
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
