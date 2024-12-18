using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private OrdinaryBullet _ordinaryBullet;
    [SerializeField] private RotationBullet _rotationBullet;
    [SerializeField] private ExplosionBullet _expsionBullet;
    [SerializeField] private Weapon _pistol;
    [SerializeField] private Weapon _mashineGun;
    [SerializeField] private Weapon _grenadeLauncher;

    private void Awake()
    {
        _ordinaryBullet = Data.Load(_ordinaryBullet, _ordinaryBullet.bulletName);
        _rotationBullet = Data.Load(_rotationBullet, _rotationBullet.bulletName);
        _expsionBullet = Data.Load(_expsionBullet, _expsionBullet.bulletName);

        _pistol = Data.Load(_pistol, _pistol.weaponName);
        _mashineGun = Data.Load(_mashineGun, _mashineGun.weaponName);
        _grenadeLauncher = Data.Load(_grenadeLauncher, _grenadeLauncher.weaponName);
    }

    private void OnApplicationQuit()
    {
        Data.Save(_ordinaryBullet, _ordinaryBullet.bulletName);
        Data.Save(_rotationBullet, _rotationBullet.bulletName);
        Data.Save(_expsionBullet, _expsionBullet.bulletName);

        Data.Save(_pistol, _pistol.weaponName);
        Data.Save(_mashineGun, _mashineGun.weaponName);
        Data.Save(_grenadeLauncher, _grenadeLauncher.weaponName);
    }
}
