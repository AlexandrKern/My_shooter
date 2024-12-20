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

    public void ButtonPress(ButtonController buttonController)
    {
        switch (buttonController.buttonType)
        {
            case ButtonType.Pause:

                break;
            case ButtonType.Start:

                break;
            case ButtonType.Audio:

                break;
            case ButtonType.UpgraddeBullets:
                GameControler.Instace.bulletManager.UpgradeBullet(buttonController);
                break;
            case ButtonType.UpgradeWeapons:
                GameControler.Instace.weponManager.UpgradeWepon(buttonController);
                break;
            default:
                break;
        }
    }


}
