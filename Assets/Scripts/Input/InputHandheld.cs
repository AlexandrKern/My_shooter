using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandheld : IInputMove, IInputShooter
{
    private bool _isShooting;
    private Joystick _inputMove;
    private Joystick _inputRotate;
    private bool _isJumpPressed;
    private bool _isNextBulletPressed;
    private bool _isNextWeaponPressed;
    private bool _isRechargePressed;
    private bool _isShootQueuePressed;
    private bool _isShootSinglePressed;

    private Button _jumpButton;
    private Button _nextBulletButton;
    private Button _nextWeponButton;
    private Button _rechargeButton;
    private Button _shootButton;
    public InputHandheld(Joystick inputMove, 
        Joystick inputRotate,
        Button jumpButton, 
        Button nextBulletButton, 
        Button nextWeponButton, 
        Button rechargeButton, 
        Button shootButton)
    {
        _inputMove = inputMove;
        _inputRotate = inputRotate;
        _jumpButton = jumpButton;
        _nextBulletButton = nextBulletButton;
        _nextWeponButton = nextWeponButton;
        _rechargeButton = rechargeButton;
        _shootButton = shootButton;

        _jumpButton.onClick.AddListener(OnJumpButtonPressed);
        _nextBulletButton.onClick.AddListener(OnNextBulletButtonPressed);
        _nextWeponButton.onClick.AddListener(OnNextWeponButtonPressed);
        _rechargeButton.onClick.AddListener(OnRechargeButtonPressed);
        _shootButton.onClick.AddListener(OnShootButtonPressed);
    }

    public Vector3 GetDirection()
    {
        float horizontal = _inputMove.Horizontal;
        float vertical = _inputMove.Vertical;

        return new Vector3(horizontal, 0, vertical);
    }

    public Vector3 GetMousePosition()
    {
        float horizontal = _inputRotate.Horizontal;
        float vertical = _inputRotate.Vertical;

        return new Vector3(horizontal, 0, vertical);
    }

    public bool IsJumpPressed()
    {
        Debug.Log("Прыгнул " + _isJumpPressed);
        return _isJumpPressed;
    }

    public bool IsNextBullet()
    {
        return _isNextBulletPressed;
    }

    public bool IsNextWepon()
    {
        Debug.Log("Поменял оружие " + _isNextWeaponPressed);
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
       return _isShootSinglePressed;
    }
    // Методы для обработки нажатий на кнопки
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

    private void OnShootButtonPressed()
    {
        _isShootSinglePressed = true;
    }

    // Вызывайте сброс флагов после их использования
    public void ResetFlags()
    {
        _isJumpPressed = false;
        _isNextBulletPressed = false;
        _isNextWeaponPressed = false;
        _isRechargePressed = false;
        _isShootSinglePressed = false;
        _isShootQueuePressed = false;
    }
}
