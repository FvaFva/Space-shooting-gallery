using System;
using System.Collections;

public interface IEnemyMoveState
{
    public bool IsMoving { get; }
    public event Action<IEnemyMoveState> Finished;
    public IEnumerator Existence();
    public void InitializeTargetState(IEnemyMoveState targetState);
}