using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rb;
    private IInputMove _input;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private float _rotationSpeed = 5;

    public static Action<Vector3> OnMove;
    public static Action<bool> OnJump;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GameManager.Instace.inputManager._currentInputMove;

    }
    private void Update()
    {
        Move(_input.GetDirection());
        Jump(_input.IsJumpPressed());
    }
    public void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // ѕоворот на 90 градусов влево (по оси Y)
            // Ќаправление остаетс€ вперед, но сам персонаж ориентирован на левую сторону.
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation * Quaternion.Euler(0, 40, 0), _rotationSpeed * Time.deltaTime);
        }

        // ƒвигаем персонажа вперед в направлении
        _rb.velocity = new Vector3(direction.x * _moveSpeed, _rb.velocity.y, direction.z * _moveSpeed);
        _rb.angularVelocity = Vector3.zero;

        OnMove?.Invoke(direction);
    }

    public void Jump(bool isButtonJumpPressed)
    {
        OnJump?.Invoke(PlayerController.Instace.CheckIsGrounded() && isButtonJumpPressed);
        if (PlayerController.Instace.CheckIsGrounded() && isButtonJumpPressed)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
       
    }
}
