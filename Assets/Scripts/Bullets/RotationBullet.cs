using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RotationBullet", menuName = "Bullets/OrdinaryBullet")]
public class RotationBullet : BulletBase
{
    public float rotationSpeed = 10;
    public float rotationDuration = 3f;
}
