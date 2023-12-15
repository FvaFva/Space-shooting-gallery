using System;
using System.Collections;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private const float DelayUpdate = 0.01f;
    private const float CheckDistance = 0.55f;
    private const float CheckRadius = 0.35f;

    [SerializeField] private LayerMask _mask;

    private Transform _transform;
    private WaitForSeconds _delay;
    private bool _isGrounded;
    private Coroutine _checking;

    public event Action<bool> Changed;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _transform = transform;
        _delay = new WaitForSeconds(DelayUpdate);
        _isGrounded = CheckGrounded();
    }

    private void OnEnable()
    {
        _checking = StartCoroutine(CheckingGround());
    }

    private void OnDisable()
    {
        StopCoroutine(_checking);
    }

    private bool CheckGrounded()
    {
        return Physics.SphereCast(_transform.position, CheckRadius, Vector3.down, out RaycastHit hit, CheckDistance, _mask, QueryTriggerInteraction.Ignore);
    }

    private IEnumerator CheckingGround()
    {
        yield return null;
        bool currentCheckResult = CheckGrounded();

        while (true)
        {
            if(_isGrounded != currentCheckResult)
            {
                _isGrounded = currentCheckResult;
                Changed?.Invoke(_isGrounded);
            }

            currentCheckResult = CheckGrounded();
            yield return _delay;
        }
    }
}