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
    [SerializeField] private EnemySettings _mutant;
    [SerializeField] private EnemySettings _vampire;
    [SerializeField] private EnemySettings _warrok;


    private void Awake()
    {
        DataPlayer.Load();
        SetBulletCount();
        _ordinaryBullet = DataScriptableObject.Load(_ordinaryBullet, _ordinaryBullet.bulletName);
        _rotationBullet = DataScriptableObject.Load(_rotationBullet, _rotationBullet.bulletName);
        _expsionBullet = DataScriptableObject.Load(_expsionBullet, _expsionBullet.bulletName);

        _pistol = DataScriptableObject.Load(_pistol, _pistol.weaponName);
        _mashineGun = DataScriptableObject.Load(_mashineGun, _mashineGun.weaponName);
        _grenadeLauncher = DataScriptableObject.Load(_grenadeLauncher, _grenadeLauncher.weaponName);

        _mutant = DataScriptableObject.Load(_mutant,_mutant.enemyName);
        _vampire = DataScriptableObject.Load(_vampire, _vampire.enemyName);
        _warrok = DataScriptableObject.Load(_warrok, _warrok.enemyName);
    }

    private void OnApplicationQuit()
    {
        DataScriptableObject.Save(_ordinaryBullet, _ordinaryBullet.bulletName);
        DataScriptableObject.Save(_rotationBullet, _rotationBullet.bulletName);
        DataScriptableObject.Save(_expsionBullet, _expsionBullet.bulletName);

        DataScriptableObject.Save(_pistol, _pistol.weaponName);
        DataScriptableObject.Save(_mashineGun, _mashineGun.weaponName);
        DataScriptableObject.Save(_grenadeLauncher, _grenadeLauncher.weaponName);
    }

    private void SetBulletCount()
    {
        _ordinaryBullet.countBullet = 50;
        _expsionBullet.countBullet = 5;
        _rotationBullet.countBullet = 10;
    }
}
