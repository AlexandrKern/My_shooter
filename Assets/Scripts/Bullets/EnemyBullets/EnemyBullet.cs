using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBullet", menuName = "Bullets/Enemy/Enemy bullet")]
public class EnemyBullet : ScriptableObject
{
    public GameObject _bulletPrefub;
    public float speed;
    public float damage;
}
