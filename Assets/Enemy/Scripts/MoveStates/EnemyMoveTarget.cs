using UnityEngine;

public struct EnemyMoveTarget
{
    private readonly Vector3 _position;
    private readonly Transform _target;
    public Vector3 Position { get => _target?.position ?? _position; }

    public EnemyMoveTarget (Transform target, Vector3 position)
    {
        _position = position;
        _target = target;
    }
}