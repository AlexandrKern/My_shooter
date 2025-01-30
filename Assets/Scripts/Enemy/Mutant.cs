public class Mutant : EnemyBase
{
    public override void Attack()
    {
        if (PlayerDetected() && canAttack)
        {
            OnAttack?.Invoke(false);
            canAttack = false;
            StartCoroutine(AttackCooldown("MutantAttack"));
        }
        FaceTarget();
    }
}
