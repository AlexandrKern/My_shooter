using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private TypeOfBullet _typeOfBullet;
    [SerializeField] private int _ammoAmount;
    public BulletBase bullet;

    private void Start()
    {
        //if (!bullet.isUnlock)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        GameManager.Instace.animationObjectsManager.AnimateObject(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instace.bulletManager.AddBullets(_ammoAmount, _typeOfBullet);
            Destroy(gameObject);
        }
    }
}
