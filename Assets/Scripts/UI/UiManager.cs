using UnityEngine;
public class UiManager : MonoBehaviour
{
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
