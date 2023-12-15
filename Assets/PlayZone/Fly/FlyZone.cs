using System.Collections.Generic;
using UnityEngine;

public class FlyZone : MonoBehaviour
{
    private const int HalfCoefficient = 2;
    private const int TryCount = 20;

    [SerializeField] private List<Collider> _colliders;
    [SerializeField] private int _countPreload;

    private List<Vector3> _points;
    private int _lastBox;

    private void Awake()
    {
        _lastBox = _colliders.Count;
        GeneratePoints();
    }

    public Vector3 GetPosition()
    {
        if (_points.Count == 0)
            return Vector3.zero;

        return _points[Random.Range(0, _points.Count - 1)];
    }

    private void GeneratePoints()
    {
        _points = new List<Vector3>();

        if (_lastBox == -1)
            return;

        for (int i = 0; i < _countPreload; ++i)
        {
            Vector3 point = GeneratePoint();

            if(point != Vector3.zero)
                _points.Add(GeneratePoint());
        }
    }

    private Vector3 GeneratePoint()
    {
        Collider collider = _colliders[Random.Range(0, _lastBox)];
        Vector3 halfBoxSize = collider.bounds.size / HalfCoefficient;
        int countTry = TryCount;

        while (countTry > 0)
        {
            countTry--;
            Vector3 point = collider.bounds.center + new Vector3(Random.Range(-halfBoxSize.x, halfBoxSize.x), Random.Range(-halfBoxSize.y, halfBoxSize.y), Random.Range(-halfBoxSize.z, halfBoxSize.z));

            if(Physics.Raycast(new Ray(point, Vector3.down * 0.03f), out RaycastHit hitInfo))
                return point;
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (Vector3 point in _points)
            Gizmos.DrawCube(point, Vector3.one / 4);
    }
}
