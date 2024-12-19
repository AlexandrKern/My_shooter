using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RotationBullet", menuName = "Bullets/RotationBullet")]
public class RotationBullet : BulletBase
{
    [BoxGroup("Rotation speed properties")]
    public int priceRotationSpeed = 100;
    [BoxGroup("Rotation speed properties")]
    public float rotationSpeed = 10;
    [BoxGroup("Rotation speed properties")]
    public float maxRotationSpeed = 100;

    [BoxGroup("Duration properties")]
    public int priceDuration = 100;
    [BoxGroup("Duration properties")]
    public float rotationDuration = 3;
    [BoxGroup("Duration properties")]
    public float maxRotationDuration = 20;
}
