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
        PlayerHealth.OnDie += Die;
    }

    private void OnDisable()
    {
        PlayerMove.OnMove -= Move;
        PlayerMove.OnJump -= Jump;
        PlayerHealth.OnDie -= Die;
    }
    private void Move(Vector3 direction)
    {
        _animator.SetBool("IsMove",direction.magnitude > 0.1);
    }

    private void Jump(bool jump)
    {
        _animator.SetBool("IsJump", jump);
    }
    private void Die(bool isDeath)
    {
        _animator.SetBool("IsDeath", isDeath);
    }
}
