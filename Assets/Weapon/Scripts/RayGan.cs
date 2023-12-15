using System;
using System.Collections;
using UnityEngine;

public class RayGan : BaseWeapon, IRechargeable
{
    private const float Delay = 0.03f;
    private const float FinalValueRecharge = 1;

    [SerializeField] private float _reloadCoefficient = 1;

    private float _rechargeProgress = FinalValueRecharge;
    private WaitForSeconds _delay;

    public event Action<float> RechargeProgressChanged;

    private void Start()
    {
        _delay = new WaitForSeconds(Delay);
    }

    protected override void ShotImpact()
    {
        StartCoroutine(Recharge());
        ShotRayCast();
    }

    protected override bool IsReadyToShot()
    {
        return _rechargeProgress == FinalValueRecharge;
    }

    private void ShotRayCast()
    {
        if (Physics.Raycast(ShotPosition, ShotForward, out RaycastHit hitInfo, MaxDistance))
        {
            HitEffect.Show(hitInfo);
            Damageable damageable = hitInfo.collider.GetComponentInParent<Damageable>();

            if (damageable != null)           
                damageable.TakeDamage(Damage);

            Rigidbody targetBody = hitInfo.rigidbody;
            
            if (targetBody != null)
            {
                targetBody.AddForceAtPosition(ShotForward * ImpactForce, hitInfo.point);
            }
        }
    }

    private IEnumerator Recharge()
    {
        _rechargeProgress = 0;
        yield return null;

        while(_rechargeProgress < FinalValueRecharge)
        {
            _rechargeProgress += Delay * _reloadCoefficient;
            RechargeProgressChanged?.Invoke(_rechargeProgress / FinalValueRecharge);
            yield return _delay;
        }

        _rechargeProgress = FinalValueRecharge;
        RechargeProgressChanged?.Invoke(FinalValueRecharge);
    }
}
