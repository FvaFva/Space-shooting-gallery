using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private const float OptimizationDelay = 0.1f;

    [SerializeField] private float _knockdown = 1f;
    [SerializeField] private float _radius;
    [SerializeField] private float _secondsDelay;
    [SerializeField] private GroundChecker _groundChecker;

    private NavMeshAgent _agent;
    private Coroutine _currentExistence;
    private IEnemyMoveState _current;
    private List<IEnemyMoveTransactions> _transactions;

    public event Action<bool> MoveStateChanged;
    public IEnemyMoveState Current => _current;

    private void Awake()
    {
        _transactions = new List<IEnemyMoveTransactions>();
    }

    private void OnEnable()
    {
        if(_current != null)
            _currentExistence = StartCoroutine(_current.Existence());

        foreach (IEnemyMoveTransactions transaction in _transactions)
            transaction.Subscribe();
    }

    private void OnDisable()
    {
        if(_currentExistence != null)
            StopCoroutine(_currentExistence);

        foreach (IEnemyMoveTransactions transaction in _transactions)
            transaction.Unsubscribe();
    }

    public void Instantiate(EnemyHealth health)
    {
        _transactions = new List<IEnemyMoveTransactions>();
        enabled = TryGetComponent(out NavMeshAgent agent);
        _agent = agent;

        InScanTargetEnemyState scanTargetEnemyState = new InScanTargetEnemyState(_radius, transform, _agent);
        InMovementEnemyState movementEnemyState = new InMovementEnemyState(OptimizationDelay, _agent, transform);
        InKnockdownEnemyState knockdownState = new InKnockdownEnemyState(_knockdown, _groundChecker);

        scanTargetEnemyState.InitializeTargetState(movementEnemyState);
        movementEnemyState.InitializeTargetState(knockdownState);
        knockdownState.InitializeTargetState(scanTargetEnemyState);

        _current = knockdownState;
        _current.Finished += ChangeState;
        _currentExistence = StartCoroutine(_current.Existence());

        EnemyKnockdownTransaction knockdown = new EnemyKnockdownTransaction(knockdownState, health);

        _transactions.Add(knockdown);
        knockdown.Triggered += ChangeState;
        knockdown.Subscribe();
    }

    public void TurnOff()
    {
        _agent.enabled = false;
        enabled = false;
    }

    private void ChangeState(IEnemyMoveState newState)
    {
        StopCoroutine(_currentExistence);
        _current.Finished -= ChangeState;
        _current = newState;
        _agent.enabled = _current.IsMoving;
        MoveStateChanged?.Invoke(_current.IsMoving);
        _current.Finished += ChangeState;
        _currentExistence = StartCoroutine(_current.Existence());
    }
}