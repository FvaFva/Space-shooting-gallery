using System;
using UnityEngine;

public abstract class BaseHealth : Damageable
{
    [SerializeField] private float _health;
    [SerializeField] private AudioSource _impactSound;
    
    public float Health => _health;

    public event Action Died;
    public event Action Hit;

    public override void TakeDamage(float damage)
    {
        if (damage <= 0)
            return;

        if (_health < 0)
            return;

        _health -= damage;
        Hit?.Invoke();

        if (_health <= 0)
        {
            _health = 0;
            Died?.Invoke();
            Die();
        }
        else
        {
            if(_impactSound != null)
                _impactSound.Play();
        }
    }

    protected abstract void Die();
}