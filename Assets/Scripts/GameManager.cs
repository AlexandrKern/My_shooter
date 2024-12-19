using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

   

    public void ButtonPress(ButtonType buttonType
        ,BulletBase bullet
        ,TypeUpgradeBullet typeUpgradeBullet
        , Weapon weapon
        ,TypeUpgradeWeapon typeUpgradeWeapon
        ,ButtonController buttonController)
    {
        switch (buttonType)
        {
            case ButtonType.Pause:

                break;
            case ButtonType.Start:

                break;
            case ButtonType.Audio:

                break;
            case ButtonType.UpgraddeBullets:
                UpgradeBullet(bullet,typeUpgradeBullet,buttonController);
                break;
            case ButtonType.UpgradeWeapons:
                UpgradeWepon(weapon,typeUpgradeWeapon,buttonController);
                break;
            default:
                break;
        }
    }

    private void UpgradeWepon(Weapon weapon,TypeUpgradeWeapon typeUpgradeWeapon,ButtonController buttonController)
    {
        PlayerControleer.Instace.weponManager.SetWeapon(weapon);
        switch (typeUpgradeWeapon)
        {
            case TypeUpgradeWeapon.RechargeTime:
                PlayerControleer.Instace.weponManager.UpgradeWeaponRechargeTime(buttonController);
                break;
            case TypeUpgradeWeapon.FireRate:
                PlayerControleer.Instace.weponManager.UpgadeWeaponFireRate(buttonController);
                break;
            case TypeUpgradeWeapon.Magazine:
                PlayerControleer.Instace.weponManager.UpgadeWeaponmMagazine(buttonController);
                break;
            default:
                break;
        }
    }
    private void UpgradeBullet(BulletBase bullet,TypeUpgradeBullet typeUpgradeBullet,ButtonController buttonController)
    {
        PlayerControleer.Instace.bulletManager.SetBullet(bullet);
        switch (typeUpgradeBullet)
        {
            case TypeUpgradeBullet.Speed:
                PlayerControleer.Instace.bulletManager.UpgradeBulletSpeed(buttonController);
                break;
            case TypeUpgradeBullet.Damage:
                PlayerControleer.Instace.bulletManager.UpgradeBulletDamage(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionRadius:
                PlayerControleer.Instace.bulletManager.UpgradeBulletExplosionRadius(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionForce:
                PlayerControleer.Instace.bulletManager.UpgradeBulletExplosionRadius(buttonController);
                break;
            case TypeUpgradeBullet.ExplosionTimer:
                PlayerControleer.Instace.bulletManager.UpgradeBulletExplosionTimer(buttonController);
                break;
            case TypeUpgradeBullet.RotationSpeed:
                PlayerControleer.Instace.bulletManager.UpgradeBulletRotationSpeed(buttonController);
                break;
            case TypeUpgradeBullet.RotationDuration:
                PlayerControleer.Instace.bulletManager.UpgradeBulletRotationDuration(buttonController);
                break;
            default:
                break;
        }
    }

}
