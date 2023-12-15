using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy: MonoBehaviour
{
    private bool _isLive = true;
    private Rigidbody _body;

    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyAnimatorRouter _animator;
    [SerializeField] private ParticleSystem _dieEffect;
    [SerializeField] private EnemyMovement _navigator;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody tempBody) == false)
            gameObject.SetActive(false);

        _body = tempBody;

        _navigator.Instantiate(_health);
    }

    private void OnEnable()
    {
        if (_isLive)
        {
            _health.Hit += OnHit;
            _health.Died += OnDie;
            _navigator.MoveStateChanged += OnMoveStateChange;
        }
        else
        {
            _health.Decayed += OnDecayed;
        }
    }

    private void OnDisable()
    {
        if (_isLive)
        {
            _health.Hit -= OnHit;
            _health.Died -= OnDie;
            _navigator.MoveStateChanged -= OnMoveStateChange;
        }
        else
        {
            _health.Decayed -= OnDecayed;
        }
    }

    private void OnHit()
    {
        _animator.Play(AnimationType.Hit);
    }

    private void  OnDie()
    {
        _isLive = false;
        _health.Hit -= OnHit;
        _health.Died -= OnDie;
        _health.Decayed += OnDecayed;
        _body.freezeRotation = false;
        _body.mass = 0.3f;
        _body.isKinematic = false;
        _body.useGravity = true;
        _dieEffect.Play();
        _navigator.MoveStateChanged -= OnMoveStateChange;
        _navigator.TurnOff();
        _animator.Play(AnimationType.Die);
    }

    private void OnDecayed()
    {
        _dieEffect.Stop();
        gameObject.SetActive(false);
    }

    private void OnMoveStateChange(bool isInMovement)
    {
        _body.isKinematic = isInMovement;
        _body.useGravity = !isInMovement;
        _animator.SetBool(AnimationType.Walk, isInMovement);
    }
}