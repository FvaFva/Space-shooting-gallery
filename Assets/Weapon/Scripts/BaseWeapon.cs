using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BaseWeapon : Interactable, IAimable
{
    private Transform _shotPoint;

    [Header("Weapon settings")]
    [SerializeField] private float _impactForce;
    [SerializeField] private float _maxDistance;
    [SerializeField] protected float Damage;
    [SerializeField] private Projectile _projectile;

    [Header("View settings")]
    [SerializeField] private List<Renderer> _picked;
    [SerializeField] private DropView _dropView;
    [SerializeField] private ShotEffect _shotEffect;
    [SerializeField] private AudioSource _shotSound;

    [Inject] private HitEffectPool _hitEffect;
    [Inject] private CameraShaker _shaker;

    protected Vector3 ShotForward => _shotPoint.forward;
    protected Vector3 ShotPosition => _shotPoint.position;
    protected float MaxDistance => _maxDistance;
    protected Projectile Bullet => _projectile;
    protected float ImpactForce => _impactForce;
    protected HitEffectPool HitEffect => _hitEffect;

    public override void Interact()
    {
        if (IsReadyToShot())
        {
            _shaker.MakeRecoil(_impactForce);
            _shotEffect?.PlayEffect();
            _shotSound.Play();
            ShotImpact();
        }
    }

    protected override void DropImpact(Vector3 forward)
    {
        _shotPoint = null;
        foreach (Renderer renderer in _picked)
            renderer.enabled = false;

        _dropView.ChangeState(true);
    }

    protected override void TakeImpact(Transform hand)
    {
        _shotPoint = hand;

        foreach (Renderer renderer in _picked)
            renderer.enabled = true;

        _dropView.ChangeState(false);
    }
    protected override void ValidateLoad()
    {
    }

    protected abstract void ShotImpact();

    protected virtual bool IsReadyToShot()
    {
        return true;
    }
}
