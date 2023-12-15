using System;

public class DamageReactive : Damageable
{
    public event Action<float> Damaged;

    public override void TakeDamage(float damage)
    {
        Damaged?.Invoke(damage);
    }
}