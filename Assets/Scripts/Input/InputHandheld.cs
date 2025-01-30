using UnityEngine;
using UnityEngine.UI;

public class InputHandheld : IInputMove, IInputShooter
{
    private bool _isShooting;
    private Joystick _inputMove;
    private bool _isJumpPressed;
    private bool _isNextBulletPressed;
    private bool _isNextWeaponPressed;
    private bool _isRechargePressed;
    private bool _isShootQueuePressed;

    private Button _jumpButton;
    private Button _nextBulletButton;
    private Button _nextWeponButton;
    private Button _rechargeButton;
    public InputHandheld(Joystick inputMove, 
        Button jumpButton, 
        Button nextBulletButton, 
        Button nextWeponButton, 
        Button rechargeButton)
    {
        _inputMove = inputMove;
        _jumpButton = jumpButton;
        _nextBulletButton = nextBulletButton;
        _nextWeponButton = nextWeponButton;
        _rechargeButton = rechargeButton;

        _jumpButton.onClick.AddListener(OnJumpButtonPressed);
        _nextBulletButton.onClick.AddListener(OnNextBulletButtonPressed);
        _nextWeponButton.onClick.AddListener(OnNextWeponButtonPressed);
        _rechargeButton.onClick.AddListener(OnRechargeButtonPressed);
    }

    public Vector3 GetDirection()
    {
        float horizontal = _inputMove.Horizontal;
        float vertical = _inputMove.Vertical;

        return new Vector3(horizontal, 0, vertical).normalized;
    }

    public Vector3 GetMousePosition()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.position.x > Screen.width * 0.4f && touch.phase == TouchPhase.Moved)
                {
                    return new Vector3(touch.deltaPosition.x, 0, 0);
                }
            }
        }
        return Vector3.zero;
    }

    public bool IsJumpPressed()
    {
        return _isJumpPressed;
    }

    public bool IsNextBullet()
    {
        return _isNextBulletPressed;
    }

    public bool IsNextWepon()
    {
        return _isNextWeaponPressed;
    }

    public bool IsRecharge()
    {
        return _isRechargePressed;
    }

    public bool IsShootQueue()
    {
        return _isShootQueuePressed;
    }

    public bool IsShootSingle()
    {
       return false;
    }
    private void OnJumpButtonPressed()
    {
        _isJumpPressed = true;
    }

    private void OnNextBulletButtonPressed()
    {
        _isNextBulletPressed = true;
    }

    private void OnNextWeponButtonPressed()
    {
        _isNextWeaponPressed = true;
    }

    private void OnRechargeButtonPressed()
    {
        _isRechargePressed = true;
    }

    public void OnShootButtonUp()
    {
        _isShootQueuePressed = false;
    }

    public void OnShootButtonDown()
    {
        _isShootQueuePressed = true;
    }

    public void ResetFlags()
    {
        _isJumpPressed = false;
        _isNextBulletPressed = false;
        _isNextWeaponPressed = false;
        _isRechargePressed = false;
    }
}
