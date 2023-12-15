using UnityEngine;
using Zenject;
using DG.Tweening;

public class Fly : MonoBehaviour
{
    private const float MicroVelocity = 0.4f;

    [SerializeField] private float _speed = 2;
    [SerializeField] private float _sleepTime = 4;

    [Inject] FlyZone _zone;

    private Vector3 _target;
    private Rigidbody _rb;
    private Tween _move; 

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody component))
            _rb = component;

        NavigateToTarget(0);
    }

    private void NavigateToTarget(float sleep)
    {
        if (_rb == null)
            return;

        _target = _zone.GetPosition();
        _move = _rb.DOMove(_target, CalculateTime())
            .SetEase(Ease.Linear)
            .SetDelay(sleep)
            .OnComplete(() => NavigateToTarget(_sleepTime))
            .OnUpdate(() => UpdateMove());
    }

    private float CalculateTime()
    {
        return Vector3.Distance(_rb.position, _target) / _speed;
    }

    private void UpdateMove()
    {
        if (_rb.velocity.sqrMagnitude < MicroVelocity)
            return;

        _move.Kill();
        DOTween.To(() => _rb.velocity, vel => _rb.velocity = vel, Vector3.zero, 1f)
            .SetDelay(1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => NavigateToTarget(0));
    }
}
