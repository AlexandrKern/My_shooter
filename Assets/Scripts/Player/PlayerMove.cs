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


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GameControler.Instace.inputManager._currentInputMove;

    }
    private void Update()
    {
        Move(_input.GetDirection());
        Jump(_input.IsJumpPressed());
    }
    public void Move(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        _rb.velocity = new Vector3(direction.x * _moveSpeed, _rb.velocity.y, direction.z * _moveSpeed);
        _rb.angularVelocity = Vector3.zero;
    }

    public void Jump(bool isButtonJumpPressed)
    {
        if (PlayerController.Instace.CheckIsGrounded() && isButtonJumpPressed)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
}
