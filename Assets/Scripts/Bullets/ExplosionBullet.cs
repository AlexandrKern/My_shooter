using UnityEngine;
[SerializeField]
[CreateAssetMenu(fileName = "ExplosionBullet", menuName = "Bullets/ExplosionBullet")]
public class ExplosionBullet : BulletBase
{
    public float explosionRadius = 10;
    public float maxExplosionRadius = 100;
    public float explosionForce = 10;
    public float maxExplosionForce = 100;
    public float explosionTimer = 3;
    public float minExplosionTimer = 0.1f;
    public LayerMask damageableLayers;
}
