using UnityEngine;
public class BulletBase : ScriptableObject
{
    public string bulletName;
    public GameObject bulletPrefub;
    public float speed = 10;
    public float maxSpeed = 100;
    public int damage = 1;
    public int maxDamage = 100;
    public TypeOfBullet typeOfBullet;
}

public enum TypeOfBullet
{
    Ordinary,
    Explosion,
    Rotation
}
