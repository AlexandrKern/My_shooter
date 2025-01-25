using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiGameSceneManager : UiBase
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _audioScreen;
    [SerializeField] private GameObject _moneyScreen;
    [SerializeField] private GameObject _inputHandheldPanel;

    [SerializeField] private Image _notificationScreen;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _bulletImage;
    [SerializeField] private Image healthImage;


    [SerializeField] private Text _playerMonyText;
    [SerializeField] private Text _notificationText;
    [SerializeField] private Text _bulletCountText;
    [SerializeField] private Text _bulletCountInMagazineText;
    [SerializeField] private Text _waveTimerText;
    [SerializeField] private Text _waveCountText;
    [SerializeField] private Text _enemyDeathCountText;


    private GameObject _currentScreen;
    private Coroutine _updateMoneyCoroutine;

    private void OnDisable()
    {
        DataPlayer.OnMoneyChanged -= ChangeMonyScreen;
        GameManager.Instace.bulletManager.OnChangeCountBullet -= ChangeBulletCountText;
        PlayerController.Instace.playerShoot.OnEndedBullet -= ShowNatitficationScreen;
        PlayerController.Instace.playerShoot.OnNextWeapon -= ChangeWeaponAnim;
        PlayerController.Instace.playerShoot.OnChangeBulletCount -= ChangeBulletCountText;
        PlayerController.Instace.playerShoot.OnChangeBulletCountInmagazine -= ChangeBulletCountInMagazineText;
        PlayerController.Instace.playerShoot.OnNextBullet -= ChangeBulletImage;
        PlayerHealth.OnHealthChanged += ChangeHealthBar;
        PlayerHealth.OnDie += OnDeathScreen;
        SceneController.Instance.enemyWaveManager.OnCangeWaveCount -= ChangeWaveCountText;
        SceneController.Instance.enemyWaveManager.OnCangeTimerValue -= ChangeWaveTimerText;
    }
    private void Start()
    {
        AudioManager.Instance.PlayMusic("GameSound");
        DataPlayer.OnMoneyChanged += ChangeMonyScreen;
        GameManager.Instace.bulletManager.OnChangeCountBullet += ChangeBulletCountText;
        PlayerController.Instace.playerShoot.OnEndedBullet += ShowNatitficationScreen;
        PlayerController.Instace.playerShoot.OnNextWeapon += ChangeWeaponAnim;
        PlayerController.Instace.playerShoot.OnChangeBulletCount += ChangeBulletCountText;
        PlayerController.Instace.playerShoot.OnChangeBulletCountInmagazine += ChangeBulletCountInMagazineText;
        PlayerController.Instace.playerShoot.OnNextBullet += ChangeBulletImage;
        PlayerHealth.OnHealthChanged += ChangeHealthBar;
        PlayerHealth.OnDie += OnDeathScreen;
        SceneController.Instance.enemyWaveManager.OnCangeWaveCount += ChangeWaveCountText;
        SceneController.Instance.enemyWaveManager.OnCangeTimerValue += ChangeWaveTimerText;
        _currentScreen = _gameScreen;
        ChangeWaveCountText(1);
        SetMoneyCount();
        HandheldPanelActivate(!GameManager.Instace.inputManager.isDesctop);

        ChangeBulletCountText(GameManager.Instace.bulletManager.GetBullet().countBullet);
        ChangeBulletCountInMagazineText(GameManager.Instace.weponManager.GetWeapon().magazine);
    }

    private void HandheldPanelActivate(bool isActiv)
    {
        _inputHandheldPanel.SetActive(isActiv);
    }

    public override void ButtonPress(ButtonController buttonController)
    {
        switch (buttonController.buttonType)
        {
            case ButtonType.End:
                ChangeScreen(_endScreen);
                SetTextResultGame();
                break;
            case ButtonType.Menu:
                LoadScene("MenuScene");
                PlayGame();
                break;
            case ButtonType.Pause:
                PauseGame();
                ChangeScreen(_pauseScreen);
                break;
            case ButtonType.Continue:
                PlayGame();
                ChangeScreen(_gameScreen);
                break;
            case ButtonType.Audio:
                ChangeScreen(_audioScreen);
                break;
            case ButtonType.Back:
                ChangeScreen(_pauseScreen);
                break;
            case ButtonType.Return:
                LoadScene("GameScene");
                PlayGame();
                break;
        }
    }

    private void ChangeWaveCountText(int waveCount)
    {
        _waveCountText.text = "Волна " + waveCount;
        GameManager.Instace.animationTextManager.AnimateWaveCountText(_waveCountText);
    }
    private void ChangeWaveTimerText(int timer)
    {
        _waveTimerText.gameObject.SetActive(timer > 0);
        _waveTimerText.text = timer.ToString();
        GameManager.Instace.animationTextManager.AnimateTimerText(_waveTimerText);

    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    private void PlayGame()
    {
        Time.timeScale = 1;
    }
    private void ChangeScreen(GameObject screen)
    {
        if(_currentScreen==null) return;
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
            case "MenuPanel":
                return true;
            case "UpgradePanels":
                return false;
            case "ShopPanel":
                return true;
            case "UpgradeBullet":
                return false;
            case "UpgradeExplosionBulletPanel":
                return true;
            case "UpgradeRotationBulletPanel":
                return true;
            case "UpgradeOrdinaryBulletPanel":
                return true;
            case "UpgradeWeapon":
                return false;
            case "UpgradePistol":
                return true;
            case "UpgradeMashineGun":
                return true;
            case "UpgradeGrenadeLauncher":
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

    private void ShowNatitficationScreen(string text)
    {
        _notificationText.text = text;
        GameManager.Instace.animationImageManager.AnimationNotification(_notificationScreen,_notificationText);

    }

    private void ChangeWeaponAnim(Sprite sprite)
    {
        _weaponImage.sprite = sprite;
        GameManager.Instace.animationImageManager.AnimationWeapon(_weaponImage);
    }

    private void ChangeBulletCountText(int countBullet)
    {
        _bulletCountText.text = $"/ {countBullet}";
    }
    private void ChangeBulletCountInMagazineText(int countMagazine)
    {
        _bulletCountInMagazineText.text = countMagazine.ToString();
    }

    private void ChangeBulletImage(Sprite sprite)
    {
        _bulletImage.sprite = sprite;
    }

    private void ChangeHealthBar(int health)
    {
        float healthPercentage = Mathf.Clamp01(health / 100f);
        GameManager.Instace.animationImageManager.AnimationUpdateHealth(healthPercentage,healthImage);
    }

    private void OnDeathScreen(bool IsDeath)
    {
        if (IsDeath)
        {
            AudioManager.Instance.PlayMusic("GameOverSound");
            ChangeScreen(_gameOverScreen);
            SetTextResultGame();
        }
    }

    

    private void SetTextResultGame()
    {
        if(_enemyDeathCountText ==  null)   return;
        _enemyDeathCountText.gameObject.SetActive(true);
        _enemyDeathCountText.text = $"Уничтожено\n" +
            $"мутантов: {EnemyManager.Instace.GetEnemyDeathCurrentCount("mutant")}\n" +
            $"вампиров: {EnemyManager.Instace.GetEnemyDeathCurrentCount("vampire")}\n" +
            $"орков: {EnemyManager.Instace.GetEnemyDeathCurrentCount("warrok")}";
    }
}
