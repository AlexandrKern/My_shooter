using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivisionBar : MonoBehaviour
{
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private Color _activColor;
    [SerializeField] private Color _inActivColor;
    [SerializeField] private bool _valueOpposite;
    private Transform _transform;
    private Image[] _divisions;
    private int _currentValue; 

    private void Start()
    {
        _transform = transform;
        InitializeBar();
        UpdateDivisions();
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
            case ButtonType.UpgraddeBullets:
                InitializeValueBulletBar();
                break;
            case ButtonType.UpgradeWeapons:
                break;

        }
    }
    private void InitializeValueBulletBar()
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
    private void UpdateDivisions()
    {
        int totalDivision = _transform.childCount;
        for (int i = totalDivision-1; i >= 0; i--)
        {
            int index = totalDivision - 1 - i;
            if(index < _currentValue)
            {
                _divisions[index].color = _activColor;
            }
            else
            {
                _divisions[index].color = _inActivColor;
            }
        }
    }
}
