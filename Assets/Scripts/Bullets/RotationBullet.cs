using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
[CreateAssetMenu(fileName = "RotationBullet", menuName = "Bullets/OrdinaryBullet")]
public class RotationBullet : BulletBase
{
    public float rotationSpeed = 10;
    public float maxRotationSpeed = 100;
    public float rotationDuration = 3;
    public float maxRotationDuration = 20;
}
