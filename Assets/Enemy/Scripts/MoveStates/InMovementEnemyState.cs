using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using System;

public class InMovementEnemyState : IEnemyMoveState
{
    private const float AcceptableMagnitudeToTarget = 0.05f;

    private NavMeshAgent _agent;
    private Transform _main;
    private WaitForSeconds _delay;
    private EnemyMoveTarget _target;
    private IEnemyMoveState _targetState;

    public InMovementEnemyState(float secondsDelay, NavMeshAgent agent, Transform transform)
    {
        _agent = agent;
        _main = transform;
        _delay = new WaitForSeconds(secondsDelay);
    }

    public event Action<IEnemyMoveState> Finished;

    public bool IsMoving => true;

    public void ApplyTarget(EnemyMoveTarget target) => _target = target;

    public IEnumerator Existence()
    {
        _agent.isStopped = false;
        yield return null;

        while ((_main.position - _target.Position).magnitude > AcceptableMagnitudeToTarget)
            yield return _delay;

        _agent.isStopped = true;
        Finished?.Invoke(_targetState);
    }

    public void InitializeTargetState(IEnemyMoveState targetState) => _targetState ??= targetState;
}