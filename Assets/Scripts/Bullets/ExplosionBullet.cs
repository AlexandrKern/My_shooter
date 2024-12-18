using UnityEngine;
[CreateAssetMenu(fileName = "ExplosionBullet", menuName = "Bullets/ExplosionBullet")]
public class ExplosionBullet : BulletBase
{
    public float explosionRadius = 10;
    public float explosionForce = 10;
    public float explosionTimer = 3;
    public LayerMask damageableLayers;
}
