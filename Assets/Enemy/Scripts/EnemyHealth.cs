using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] private float _secondsDecaying = 10;
    public event Action Decayed;

    protected override void Die()
    {
        StartCoroutine(DecayingDelay());
    }

    private IEnumerator DecayingDelay()
    {
        yield return new WaitForSeconds(_secondsDecaying);
        Decayed?.Invoke();
    }
}