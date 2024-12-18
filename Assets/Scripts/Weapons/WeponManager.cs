using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WeponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    private Weapon[] _newWeapons;
    private List<GameObject> _weaponOjects = new List<GameObject>(); 
    private Weapon _currentWeapon;
    private Transform _weponPosition;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
         InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        _weponPosition = PlayerControleer.Instace.weaponPosition;
        foreach (Weapon weapon in weapons)
        {
            GameObject newWeapon = Instantiate(weapon.weaponPrefub, _weponPosition.position, Quaternion.identity);
            newWeapon.transform.SetParent(_weponPosition);
            newWeapon.SetActive(false);
            weapon.firePoint = newWeapon.transform.Find("FirePoint"); 
            _weaponOjects.Add(newWeapon);
        }
        _newWeapons = weapons;
        _currentWeapon = _newWeapons[0];
    }

    public Weapon GetWeapon()
    {
        _weaponOjects[_currentWeaponIndex].SetActive(true);
        return _currentWeapon;
    }
    public void NextWepon()
    {
        _weaponOjects[_currentWeaponIndex].SetActive(false);
        _currentWeaponIndex = (_currentWeaponIndex - 1 + _newWeapons.Length) % _newWeapons.Length;
        _currentWeapon = _newWeapons[_currentWeaponIndex];
    }




}
