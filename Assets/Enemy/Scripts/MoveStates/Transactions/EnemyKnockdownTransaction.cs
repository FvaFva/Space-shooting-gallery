using System;

public class EnemyKnockdownTransaction: IEnemyMoveTransactions
{
    private InKnockdownEnemyState _knockdown;
    private EnemyHealth _health;

    public EnemyKnockdownTransaction(InKnockdownEnemyState knockdown, EnemyHealth health)
    {
        _knockdown = knockdown;
        _health = health;
    }

    public event Action<IEnemyMoveState> Triggered;

    public void Subscribe()
    {
        _health.Hit += Trigger;
    }

    public void Unsubscribe()
    {
        _health.Hit -= Trigger;
    }

    private void Trigger()
    {
        Triggered?.Invoke(_knockdown);
    }
}