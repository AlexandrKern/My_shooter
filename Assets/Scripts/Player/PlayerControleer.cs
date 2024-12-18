using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControleer : MonoBehaviour
{
    #region Singleton
    public static PlayerControleer Instace;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
            playerTransform = transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public InputManager inputManager;
    public WeponManager weponManager;
    public BulletManager bulletManager;

    public Transform weaponPosition;

    [SerializeField]private LayerMask groundMask;
    [SerializeField]private float groundCheckDistance;

    [HideInInspector] public Transform playerTransform;
    public bool CheckIsGrounded()
    {
        return Physics.Raycast(playerTransform.position,Vector3.down, groundCheckDistance, groundMask);
    }

}
