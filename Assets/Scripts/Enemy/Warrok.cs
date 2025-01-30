public class Warrok : EnemyBase
{
    public override void Attack()
    {
        if (PlayerDetected() && canAttack)
        {
            OnAttack?.Invoke(false);
            canAttack = false;
            StartCoroutine(AttackCooldown("WarrokAttack"));
        }
        FaceTarget();
    }
}
