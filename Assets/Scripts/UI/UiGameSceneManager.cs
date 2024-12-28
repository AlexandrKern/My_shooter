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
        _currentScreen = _gameScreen;
        SetMoneyCount();
    }

    public override void ButtonPress(ButtonController buttonController)
    {
        switch (buttonController.buttonType)
        {
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
}
