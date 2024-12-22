using UnityEngine;
using UnityEngine.SceneManagement;
public class UiMenuManager : UiBase
{
    [SerializeField] private GameObject _upgradeScreen;
    [SerializeField] private GameObject _upgradeWeaponScreen;
    [SerializeField] private GameObject _upgradeBulletScreen;
    [SerializeField] private GameObject _upgradeExplosionScreen;
    [SerializeField] private GameObject _upgradeOrdinaryBulletScreen;
    [SerializeField] private GameObject _upgradeRotationBulletScreen;
    [SerializeField] private GameObject _upgradePistolScreen;
    [SerializeField] private GameObject _upgradeMashineGunScreen;
    [SerializeField] private GameObject _upgradeGrenadeLauncherScreen;
    [SerializeField] private GameObject _menuScreen;
    private GameObject _currentScreen;


    private void Start()
    {
        _currentScreen = _menuScreen;
    }

    public override void ButtonPress(ButtonController buttonController)
    {
        switch (buttonController.buttonType)
        {
            case ButtonType.UpgradeBulletScreen:
                ChangeScreen(_upgradeBulletScreen);
                break;
            case ButtonType.UpgradeWeaponScreen:
                ChangeScreen(_upgradeWeaponScreen);
                break;
            case ButtonType.Menu:
                ChangeScreen(_menuScreen);
                break;
            case ButtonType.ExplosionBullet:
                ChangeScreen(_upgradeExplosionScreen);
                break;
            case ButtonType.RotationBullet:
                ChangeScreen(_upgradeRotationBulletScreen);
                break;
            case ButtonType.OrdinaryBullet:
                ChangeScreen(_upgradeOrdinaryBulletScreen);
                break;
            case ButtonType.Pistol:
                ChangeScreen(_upgradePistolScreen);
                break;
            case ButtonType.MashineGun:
                ChangeScreen(_upgradeMashineGunScreen);
                break;
            case ButtonType.GrenadeLauncher:
                ChangeScreen(_upgradeGrenadeLauncherScreen);
                break;
            case ButtonType.Pause:

                break;
            case ButtonType.Start:
                LoadScene("GameScene");

                break;
            case ButtonType.Upgrades:
                ChangeScreen(_upgradeScreen);

                break;
            case ButtonType.Audio:

                break;
            case ButtonType.UpgraddeBullets:
                GameManager.Instace.bulletManager.UpgradeBullet(buttonController);
                break;
            case ButtonType.UpgradeWeapons:
                GameManager.Instace.weponManager.UpgradeWepon(buttonController);
                break;
            default:
                break;
        }
    }

    private void ChangeScreen(GameObject screen)
    {
        _currentScreen.SetActive(false);
        screen.SetActive(true);
        _currentScreen = screen;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
