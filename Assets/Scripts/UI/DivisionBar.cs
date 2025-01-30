using System;
using UnityEngine;
using UnityEngine.UI;

public class DivisionBar : MonoBehaviour
{
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private Color _activColor;
    [SerializeField] private Color _inActivColor;
    private Transform _transform;
    private Image[] _divisions;
    private int _currentValue; 

    private void Start()
    {
        _transform = transform;
        InitializeBar();
        UpdateDivisions();
        GameManager.Instace.bulletManager.OnBulletUpgrade += (buttonController, money) =>
        {
            if (buttonController == this.buttonController)
            {
                AddDivision();
            }
        };
        GameManager.Instace.weponManager.OnWeaponUpgrade += (buttonController, money) =>
        {
            if (buttonController == this.buttonController)
            {
                AddDivision();
            }
        };
    }

    private void InitializeBar()
    {
        _divisions = new Image[_transform.childCount];
        for (int i = 0; i < _transform.childCount; i++)
        {
           _divisions[i] = _transform.GetChild(i).GetComponent<Image>();
        }

        switch (buttonController.buttonType)
        {
            case ButtonType.UpgradeBullets:
                SetCurrentValueBulletBar();
                break;
            case ButtonType.UpgradeWeapons:
                SetCurrentValueWeaponBar();
                break;

        }
    }
    private void SetCurrentValueBulletBar()
    {
        Func<BulletBase, int> valueSelector = buttonController.typeUpgradeBullet switch
        {
            TypeUpgradeBullet.Speed => b => b.countOfUpgradesSpeed,
            TypeUpgradeBullet.Damage => b => b.countOfUpgradesDamage,
            TypeUpgradeBullet.ExplosionTimer => b => (b as ExplosionBullet)?.countOfUpgradesTimer ?? 0,
            TypeUpgradeBullet.ExplosionForce => b => (b as ExplosionBullet)?.countOfUpgradesForce ?? 0,
            TypeUpgradeBullet.ExplosionRadius => b => (b as ExplosionBullet)?.countOfUpgradesRadius ?? 0,
            TypeUpgradeBullet.RotationDuration => b => (b as RotationBullet)?.countOfUpgradesDuration ?? 0,
            TypeUpgradeBullet.RotationSpeed => b => (b as RotationBullet)?.countOfUpgradesRotationSpeed ?? 0,
            _=>null
        };

        if(valueSelector != null)
        {
            _currentValue = valueSelector(buttonController.bullet);
        }
    }

    private void SetCurrentValueWeaponBar()
    {
        switch (buttonController.typeWeaponUpgrade)
        {
            case TypeUpgradeWeapon.RechargeTime:
                _currentValue = buttonController.weapon.countOfUpgradesRechargeTime;
                break;
            case TypeUpgradeWeapon.FireRate:
                _currentValue = buttonController.weapon.countOfUpgradesFireRate;
                break;
            case TypeUpgradeWeapon.Magazine:
                _currentValue = buttonController.weapon.countOfUpgradesMagazine;
                break;
        }
    }
    private void UpdateDivisions()
    {
        int totalDivision = _divisions.Length;
        int filledCount = totalDivision - _currentValue;

        for (int i = 0; i < totalDivision; i++)
        {
            if (i < filledCount)
            {
                _divisions[i].color = _activColor;
            }
            else
            {
                _divisions[i].color = _inActivColor;
            }
        }
    }
    private void AddDivision()
    {
        switch (buttonController.buttonType)
        {
            case ButtonType.UpgradeBullets:
                SetCurrentValueBulletBar();
                break;
            case ButtonType.UpgradeWeapons:
                SetCurrentValueWeaponBar();
                break;
        }
        int totalDivisions = _divisions.Length;
        int targetIndex = totalDivisions - 1 - _currentValue;
        if (targetIndex >= 0 && targetIndex < totalDivisions)
        {
            GameManager.Instace.animationImageManager.AnimationImageDivision(_divisions[targetIndex]);
            
        }
    }

}
