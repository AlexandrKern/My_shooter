using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private float rotationSpeed = 5f;

    private bool isDesctop;


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
        isDesctop = GameManager.Instace.inputManager.isDesctop;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!_isDeath)
        {
            Vector3 direction = _input.GetDirection();
            Move(direction);
            Rotate(_input.GetMousePosition());
            Jump(_input.IsJumpPressed());

            if (_input is InputHandheld handheld)
            {
                handheld.ResetFlags();
            }
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

    public void Rotate(Vector3 rotateDirection)
    {
        if (Time.timeScale == 0) return;

        if (isDesctop)
        {
           RotateUsingMouse(rotateDirection);
        }
        else
        {
           RotateUsingTouch(rotateDirection);
        }
       
    }

    private void RotateUsingMouse(Vector3 mousePosition)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            Vector3 lookDirection = lookPoint - transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotationToMause = Quaternion.LookRotation(lookDirection);


                if (_rotationOffsetAngle > 0f)
                {
                    float offsetAngle = _rotationOffsetAngle;
                    Quaternion offsetRotation = Quaternion.Euler(0, offsetAngle, 0);
                    targetRotationToMause = targetRotationToMause * offsetRotation;
                }

                transform.rotation = targetRotationToMause;
            }
        }
    }
    private void RotateUsingTouch(Vector3 touchPosition)
    {
        //управление телефоном
        if (touchPosition == Vector3.zero) return;

        Quaternion faceOffsetRotation = Quaternion.Euler(0, _rotationOffsetAngle, 0);
        Vector3 adjustedDirection = faceOffsetRotation * touchPosition;

        Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Jump(bool isButtonJumpPressed)
    {
        OnJump?.Invoke(_playerController.CheckIsGrounded() && isButtonJumpPressed);
        if (_playerController.CheckIsGrounded() && isButtonJumpPressed)
        {
            AudioManager.Instance.PlaySFX("Jump");
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void SetIsDeath(bool isDeath)
    {
        _isDeath = isDeath;
    }
}

