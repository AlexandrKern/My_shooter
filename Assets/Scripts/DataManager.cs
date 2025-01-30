using System.IO;
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
        //ClearSaveData();
        DataPlayer.Load();
        SetBulletCount();
        _ordinaryBullet = DataScriptableObject.Load(_ordinaryBullet, _ordinaryBullet.bulletName);
        _rotationBullet = DataScriptableObject.Load(_rotationBullet, _rotationBullet.bulletName);
        _expsionBullet = DataScriptableObject.Load(_expsionBullet, _expsionBullet.bulletName);

        _pistol = DataScriptableObject.Load(_pistol, _pistol.weaponName);
        _mashineGun = DataScriptableObject.Load(_mashineGun, _mashineGun.weaponName);
        _grenadeLauncher = DataScriptableObject.Load(_grenadeLauncher, _grenadeLauncher.weaponName);

        _mutant = DataScriptableObject.Load(_mutant, _mutant.enemyName);
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

    void ClearSaveData()
    {
        string path = Application.persistentDataPath;

        if (Directory.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true); // true - рекурсивное удаление всех вложенных файлов и папок
            }

            Debug.Log("Все сохраненные файлы удалены.");
        }
        else
        {
            Debug.LogWarning("Папка сохранений не найдена.");
        }
    }
}
