using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InScanTargetEnemyState : IEnemyMoveState
{
    private InMovementEnemyState _targetState;
    private Transform _main;
    private NavMeshAgent _agent;
    private float _radius;

    public InScanTargetEnemyState(float radius, Transform main, NavMeshAgent agent)
    {
        _agent = agent;
        _main = main;
        _radius = radius;
    }

    public bool IsMoving => true;

    public event Action<IEnemyMoveState> Finished;

    public IEnumerator Existence()
    {
        yield return null;

        bool isCorrectTarget = false;
        int allAreas = NavMesh.AllAreas;
        Vector3 newTargetPoint = Vector3.zero;
        NavMeshPath path = new NavMeshPath();
        NavMeshPathStatus correct = NavMeshPathStatus.PathComplete;

        while (isCorrectTarget == false)
        {
            yield return null;
            newTargetPoint = UnityEngine.Random.insideUnitSphere * _radius + _main.position;
            NavMesh.SamplePosition(newTargetPoint, out NavMeshHit hit, _radius, allAreas);
            newTargetPoint = hit.position;
            if (newTargetPoint.y < float.MaxValue)
            {
                _agent.CalculatePath(newTargetPoint, path);
                isCorrectTarget = path.status == correct;
            }
        }

        _targetState.ApplyTarget(new EnemyMoveTarget(null, newTargetPoint));
        _agent.SetDestination(newTargetPoint);
        Finished?.Invoke(_targetState);
    }

    public void InitializeTargetState(IEnemyMoveState targetState) => _targetState ??= targetState as InMovementEnemyState;
}