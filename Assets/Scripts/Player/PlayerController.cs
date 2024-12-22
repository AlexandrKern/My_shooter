using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instace;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            playerTransform = transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;

    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public bool isMove;
    [HideInInspector] public bool isShoot;

    public Transform weaponPosition;

    public bool CheckIsGrounded()
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, groundCheckDistance, groundMask);
    }

}
