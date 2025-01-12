using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rb;
    private IInputMove _input;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField, Range(0, 180)]
    private float _rotationOffsetAngle = 0f;

    public static Action<Vector3> OnMove;
    public static Action<bool> OnJump;

    private bool _isDeath = false;
    private Camera _mainCamera;

    private PlayerController _playerController;

    private void OnEnable()
    {
        PlayerHealth.OnDie += SetIsDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDie -= SetIsDeath;
    }

    private void Start()
    {
        _playerController = PlayerController.Instace;
        _rb = GetComponent<Rigidbody>();
        _input = GameManager.Instace.inputManager._currentInputMove;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!_isDeath)
        {
            Vector3 direction = _input.GetDirection();
            Move(direction);
            RotateToMouse(_input.GetMousePosition());
            Jump(_input.IsJumpPressed());
        }
    }

    public void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.z).normalized;
            _rb.velocity = new Vector3(moveDirection.x * _moveSpeed, _rb.velocity.y, moveDirection.z * _moveSpeed);
        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }

        OnMove?.Invoke(direction);
    }

    public void RotateToMouse(Vector3 mausePosition)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
        Ray ray = _mainCamera.ScreenPointToRay(mausePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            Vector3 lookDirection = lookPoint - transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

              
                if (_rotationOffsetAngle > 0f)
                {
                    float offsetAngle = _rotationOffsetAngle;
                    Quaternion offsetRotation = Quaternion.Euler(0, offsetAngle, 0); 
                    targetRotation = targetRotation * offsetRotation;
                }

                transform.rotation = targetRotation;
            }
        }
    }

    public void Jump(bool isButtonJumpPressed)
    {
        OnJump?.Invoke(_playerController.CheckIsGrounded() && isButtonJumpPressed);
        if (_playerController.CheckIsGrounded() && isButtonJumpPressed)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void SetIsDeath(bool isDeath)
    {
        _isDeath = isDeath;
    }
}

