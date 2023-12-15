using System;
using System.Collections;
using UnityEngine;

public class InKnockdownEnemyState : IEnemyMoveState
{
    private const float GroundCheckDelay = 0.1f;

    private IEnemyMoveState _state;
    private WaitForSeconds _sleep;
    private WaitForSeconds _groundCheckDelay;
    private GroundChecker _groundChecker;

    public InKnockdownEnemyState(float knockdownTime, GroundChecker groundChecker)
    {
        _sleep = new WaitForSeconds(knockdownTime);
        _groundCheckDelay = new WaitForSeconds(GroundCheckDelay);
        _groundChecker = groundChecker;
    }

    public bool IsMoving => false;

    public event Action<IEnemyMoveState> Finished;

    public IEnumerator Existence()
    {
        yield return null;
        yield return _sleep;

        while (_groundChecker.IsGrounded == false)
            yield return _groundCheckDelay;
        
        Finished?.Invoke(_state);
    }

    public void InitializeTargetState(IEnemyMoveState targetState) => _state = targetState;
}