using UnityEngine;
public class BulletBase : ScriptableObject
{
    public GameObject bulletPrefub;
    public float speed = 10;
    public int damage = 1;
    public TypeOfBullet typeOfBullet;
}

public enum TypeOfBullet
{
    Ordinary,
    Explosion,
    Rotation
}
