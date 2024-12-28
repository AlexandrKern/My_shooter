using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    [SerializeField] private GameObject _shopScreen;
    [SerializeField] private GameObject _moneyScreen;
    [SerializeField] private GameObject _audioScreen;
    [SerializeField] private Text _playerMonyText;
   

    private GameObject _currentScreen;
    private Coroutine _updateMoneyCoroutine;


    private void OnEnable()
    {
        DataPlayer.OnMoneyChanged += ChangeMonyScreen;
    }

    private void OnDisable()
    {
        DataPlayer.OnMoneyChanged -= ChangeMonyScreen;
    }
    private void Start()
    {
        _currentScreen = _menuScreen;
        SetMoneyCount();
    }

    public override void ButtonPress(ButtonController buttonController)
    {
        switch (buttonController.buttonType)
        {
            case ButtonType.Shop:
                ChangeScreen(_shopScreen);
                break;
            case ButtonType.UnlockWeapons:
                GameManager.Instace.weponManager.UnlockWeapon(buttonController);
                break;
            case ButtonType.UnlockBullets:
                GameManager.Instace.bulletManager.UnlockBullet(buttonController);
                break;
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
                ChangeScreen(_audioScreen);
                break;
            case ButtonType.UpgradeBullets:
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
        _moneyScreen.SetActive(MoneyScreenActivate(_currentScreen.name));
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ChangeMonyScreen(int money)
    {
        if (_updateMoneyCoroutine != null)
        {
            StopCoroutine(_updateMoneyCoroutine);
        }

        _updateMoneyCoroutine = StartCoroutine(AnimateMoneyChange(money));
    }
    private bool MoneyScreenActivate(string screenName)
    {
        switch (screenName)
        {
            case "GamePanel":
             return true;
            default:
                return true; 
        }
    }

    private void SetMoneyCount()
    {
        _playerMonyText.text = DataPlayer.GetMoney().ToString();
    }

    private IEnumerator AnimateMoneyChange(int targetMoney)
    {
        int currentMoney = int.Parse(_playerMonyText.text);

        float duration = 0.5f; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            int displayedMoney = Mathf.RoundToInt(Mathf.Lerp(currentMoney, targetMoney, progress));
            _playerMonyText.text = displayedMoney.ToString();

            yield return null; 
        }

        _playerMonyText.text = targetMoney.ToString();
    }

}
