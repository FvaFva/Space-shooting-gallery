using System;

public interface IEnemyMoveTransactions
{
    public event Action<IEnemyMoveState> Triggered;
    public void Subscribe();
    public void Unsubscribe();
}