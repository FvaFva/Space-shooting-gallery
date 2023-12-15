using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FlyFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;
    [SerializeField] private DamageReactive _dameTaker;
    [SerializeField] private GameObject _instantiatePosition;
    [SerializeField] private ParticleSystem _spawnEffect;

    [Inject] private DiContainer _container;

    private int _countPrefabs;

    private void Awake()
    {
        _countPrefabs = _prefabs.Count;
    }

    private void OnEnable()
    {
        _dameTaker.Damaged += OnDamageable;
    }

    private void OnDisable()
    {
        _dameTaker.Damaged -= OnDamageable;
    }

    private void OnDamageable(float damage)
    {
        _spawnEffect.Play();
        GameObject newPrefab = _container.InstantiatePrefab(_prefabs[Random.Range(0, _countPrefabs)], _instantiatePosition.transform.position, Quaternion.identity, null);
    }
}
