using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance;

    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public bool isMove;
    [HideInInspector] public bool isShoot;
    private List<Weapon> weapons;
    private List<GameObject> _weaponOjects = new List<GameObject>();
    private int  _currentWeaponIndex;
    public PlayerShoot playerShoot;
    public Transform weaponPosition;

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

    private void Start()
    {
        weapons = GameManager.Instace.weponManager.unlockWeaponList;
        EquipWeapon();
       
    }

    public bool CheckIsGrounded()
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, groundCheckDistance, groundMask);
    }

    private void EquipWeapon()
    {
        foreach (Weapon weapon in weapons)
        {
            GameObject newWeapon = Instantiate(weapon.weaponPrefub, weaponPosition.position, Quaternion.identity);
            newWeapon.transform.SetParent(weaponPosition);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
            newWeapon.SetActive(false);
            weapon.firePoint = newWeapon.transform.Find("FirePoint");
            _weaponOjects.Add(newWeapon);
        }
        _weaponOjects[0].SetActive(true);
    }


    public void ChangeWeapon()
    {
        _weaponOjects[_currentWeaponIndex].SetActive(false);
        _currentWeaponIndex = (_currentWeaponIndex - 1 + _weaponOjects.Count) % _weaponOjects.Count;
        _weaponOjects[_currentWeaponIndex].SetActive(true);
    }
}
