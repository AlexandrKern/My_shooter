using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mutant : EnemyBase
{
    public override void Attack()
    {
        if (PlayerDetected() && canAttack)
        {
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
        FaceTarget();
    }
}
