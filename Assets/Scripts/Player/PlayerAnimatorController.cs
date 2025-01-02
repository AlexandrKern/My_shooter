using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        PlayerMove.OnMove += Move;
        PlayerMove.OnJump += Jump;
    }

    private void OnDisable()
    {
        PlayerMove.OnMove -= Move;
        PlayerMove.OnJump -= Jump;
    }
    private void Move(Vector3 direction)
    {
        _animator.SetBool("IsMove",direction.magnitude > 0.1);

        Debug.Log(direction.magnitude);
    }

    private void Jump(bool jump)
    {
        _animator.SetBool("IsJump", jump);
    }
}
