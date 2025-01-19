using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button button;
    private Image image;

    public ButtonType buttonType;

    [ShowIf("IsButtonTextValid")]
    public Text buttonText;

    [ShowIf("IsUpgradeBullet")]
    public BulletBase bullet;
    [ShowIf("IsBulletUpgradeValid")]
    public TypeUpgradeBullet typeUpgradeBullet;

    [ShowIf("IsUpgradeWeapon")]
    public Weapon weapon;
    [ShowIf("buttonType", ButtonType.UpgradeWeapons)]
    public TypeUpgradeWeapon typeWeaponUpgrade;

    private void OnEnable()
    {
        InitializeButtonText();
    }


    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        InitializeButtonText();
        SetButtonAnimation();
        GameManager.Instace.weponManager.OnWeaponUnlock += (buttonController, isUnlock,price) =>
        {
            if (buttonController == this)
            {
                SetEnabledItemButton(isUnlock, price);
                SetEnabledUnlockButton(isUnlock,price);
            }
        };
        GameManager.Instace.bulletManager.OnBulletUnlock += (buttonController, isUnlock,price) =>
        {
            if (buttonController == this)
            {
                SetEnabledItemButton(isUnlock,price);
                SetEnabledUnlockButton(isUnlock, price);
            }
        };
        GameManager.Instace.bulletManager.OnBulletUpgrade += (buttonController, money) =>
        {
            if (buttonController == this)
            {
                SetEnabledUpgradeButton(money);
            }
        };
        GameManager.Instace.weponManager.OnWeaponUpgrade += (buttonController, money) =>
        {
            if (buttonController == this)
            {
                SetEnabledUpgradeButton(money);
            }
        };
    }

    private void SetButtonAnimation()
    {
        switch (buttonType)
        {
            case ButtonType.Menu:
                break;
            case ButtonType.Pause:
                break;
            case ButtonType.Start:
                GameManager.Instace.animationButtonManager.ButtonPulsation(button);
                break;
            case ButtonType.Shop:
                break;
            case ButtonType.Upgrades:
                break;
            case ButtonType.UpgradeWeaponScreen:
                break;
            case ButtonType.UpgradeBulletScreen:
                break;
            case ButtonType.ExplosionBullet:
                break;
            case ButtonType.OrdinaryBullet:
                break;
            case ButtonType.RotationBullet:
                break;
            case ButtonType.Pistol:
                break;
            case ButtonType.MashineGun:
                break;
            case ButtonType.GrenadeLauncher:
                break;
            case ButtonType.Audio:
                break;
            case ButtonType.UpgradeBullets:
                break;
            case ButtonType.UpgradeWeapons:
                break;
            case ButtonType.UnlockBullets:
                break;
            case ButtonType.UnlockWeapons:
                break;
            default:
                break;
        }
    }
    public void ButtonClicked() 
    { 
        SceneController.Instance.uiBase.ButtonPress(this);
        GameManager.Instace.animationButtonManager.ButtonChangeScale(button);
        //AudioManager.Instance.PlaySFX("Click");
    }

    #region Initialize button text

    private void InitializeButtonText()
    {
        switch (buttonType)
        {
            case ButtonType.UnlockBullets:
                SetEnabledItemButton(!bullet.isUnlock,bullet.priceBullet);
                break;
            case ButtonType.UnlockWeapons:
                SetEnabledItemButton(!weapon.isUnlock,weapon.priceWeapon);
                break;
            case ButtonType.MashineGun:
                SetEnabledItemButton(weapon.isUnlock,weapon.priceWeapon);
                break;
            case ButtonType.GrenadeLauncher:
                SetEnabledItemButton(weapon.isUnlock, weapon.priceWeapon);
                break;
            case ButtonType.RotationBullet:
                SetEnabledItemButton(bullet.isUnlock,bullet.priceBullet);
                break;
            case ButtonType.ExplosionBullet:
                SetEnabledItemButton(bullet.isUnlock, bullet.priceBullet);
                break;
            case ButtonType.UpgradeBullets:
                InitializeUpgradeBulletButton();
                break;
            case ButtonType.UpgradeWeapons:
                InitializeUpgradeWeponButton();
                break;
        }
    }
    private void InitializeUpgradeBulletButton()
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
            SetEnabledUpgradeButton(price);
        }
    }
    private void InitializeUpgradeWeponButton()
    {
        if (buttonType == ButtonType.UpgradeWeapons)
        {
            switch (typeWeaponUpgrade)
            {
                case TypeUpgradeWeapon.RechargeTime:
                    SetEnabledUpgradeButton(weapon.priceRechargeTime);
                    break;
                case TypeUpgradeWeapon.FireRate:
                    SetEnabledUpgradeButton(weapon.priceFireRate);
                    break;
                case TypeUpgradeWeapon.Magazine:
                    SetEnabledUpgradeButton(weapon.priceMagazine);
                    break;
                default:
                    break;
            }
        }
    }
    private void SetEnabledUpgradeButton(int money) 
    {

        bool isMaxUpgrade = IsMaxUpgrade();

        buttonText.text = isMaxUpgrade ? "Max" : money.ToString();

        if(button == null&& image == null)return;
        if (isMaxUpgrade)
        {
            button.enabled = false;
            Color color = image.color;
            color.a = 0.7f;
            image.color = color;
        }
        else
        {
            button.enabled = true;
            Color color = image.color;
            color.a = 1f; 
            image.color = color;
        }

    }
    private void SetEnabledItemButton(bool isUnlock,int price)
    {
        if(image ==  null)return;
        if (isUnlock)
        {
            Color color = image.color;
            button.enabled = isUnlock;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Color color = image.color;
            button.enabled = isUnlock;
            color.a = 0.7f;
            image.color = color;
        }
        SetShopButtonText(isUnlock,price);
    }
    private void SetEnabledUnlockButton(bool isUnlock, int price)
    {
        Color color = image.color;
        button.enabled = !isUnlock;
        color.a = 0.3f;
        image.color = color;
        SetShopButtonText(!isUnlock,price);
    }
    private void SetShopButtonText(bool isUnlock,int price)
    {
        if (buttonText == null) return;
        if (isUnlock)
        {
            buttonText.text = price.ToString();
        }
        else
        {
            buttonText.text = "Купленно";
        }
        
    }
    private bool IsMaxUpgrade()
    {
        switch (buttonType)
        {
            case ButtonType.UpgradeBullets:
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
        if (buttonType == ButtonType.UpgradeBullets && bullet != null)
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
        return buttonType == ButtonType.UpgradeBullets && bullet != null && IsUpgradeValidForBullet(typeUpgradeBullet, bullet);
    }

    private bool IsUpgradeValidForBullet(TypeUpgradeBullet upgradeType, BulletBase bullet)
    {
        return bullet.SupportedUpgrades.Contains(upgradeType);
    }

    private bool IsButtonTextValid()
    {
        return buttonType == ButtonType.UpgradeBullets ||buttonType == ButtonType.UpgradeWeapons||buttonType == ButtonType.UnlockWeapons||buttonType==ButtonType.UnlockBullets;
    }

    private bool IsUpgradeBullet()
    {
        switch (buttonType)
        {
            case ButtonType.UnlockBullets:
                return true;
            case ButtonType.ExplosionBullet:
                return true;
            case ButtonType.OrdinaryBullet:
                return true;
            case ButtonType.RotationBullet:
                return true;
            case ButtonType.UpgradeBullets:
                return true;
            default:
                return false;
        }
    }

    private bool IsUpgradeWeapon()
    {
        switch (buttonType)
        {
            case ButtonType.UnlockWeapons:
                return true;
            case ButtonType.Pistol:
                return true;
            case ButtonType.MashineGun:
                return true;
            case ButtonType.GrenadeLauncher:
                return true;
            case ButtonType.UpgradeWeapons:
                return true;
            default:
                return false;
        }
    }

    #endregion
}
public enum ButtonType
{
    Menu,
    Pause,
    Start,
    Shop,
    Upgrades,
    UpgradeWeaponScreen,
    UpgradeBulletScreen,
    ExplosionBullet,
    OrdinaryBullet,
    RotationBullet,
    Pistol,
    MashineGun,
    GrenadeLauncher,
    Audio,
    UpgradeBullets,
    UpgradeWeapons,
    UnlockBullets,
    UnlockWeapons,
    Continue,
    Back,
    Return,
    End
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
