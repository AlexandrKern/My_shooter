using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public IInputMove _currentInputMove;
    public IInputShooter _currentInputShoot;
    [HideInInspector] public bool isDesctop;
    #region UI Handheld
    public Joystick inputMove;
    public Button jumpButton;
    public Button nextBulletButton;
    public Button nextWeponButton;
    public Button rechargeButton;
    public Button shootButton;
    #endregion


    private void Awake()
    {
        InitializeInput();
    }
    private void InitializeInput()
    {

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:
                _currentInputMove = new InputKeyBorad();
                _currentInputShoot = new InputKeyBorad();
                isDesctop = true;
                break;
            case DeviceType.Handheld:
                _currentInputMove = new InputHandheld(inputMove, jumpButton, nextBulletButton, nextWeponButton, rechargeButton);
                _currentInputShoot = new InputHandheld(inputMove, jumpButton, nextBulletButton, nextWeponButton, rechargeButton);
                isDesctop = false;
                break;


        }
    }
}
