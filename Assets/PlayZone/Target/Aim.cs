using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SpringJoint))]
public class Aim : MonoBehaviour
{
    private const float SecondsDelay = 0.03f;

    [SerializeField] private Breakable _healthHook;
    [SerializeField] private AimRopePoint _hookRopePoint;
    [SerializeField] private AimRopePoint _aimRopePoint;
    [SerializeField] private LineRenderer _rope;

    private Rigidbody _aimBody;
    private Coroutine _ropeRendering;
    private WaitForSeconds _delay;

    private void Awake()
    {
        _rope.positionCount = 2;
        _delay = new WaitForSeconds(SecondsDelay);

        if (TryGetComponent<Rigidbody>(out Rigidbody tempBody) == false)
            gameObject.SetActive(false);

        _aimBody = tempBody;
    }

    private void OnEnable()
    {
        _healthHook.Died += OnHookDied;

        if (_rope.positionCount == 2)
            _ropeRendering = StartCoroutine(RopeRender());
    }

    private void OnDisable()
    {
        _healthHook.Died -= OnHookDied;

        if (_rope.positionCount == 2)
            StopCoroutine(_ropeRendering);
    }

    private IEnumerator RopeRender()
    { 
        yield return null;

        while(true)
        {
            _rope.SetPosition(0, _aimRopePoint.Position);
            _rope.SetPosition(1, _hookRopePoint.Position);
            yield return _delay;
        }
    }

    private void OnHookDied()
    {
        BreakConnect();
    }

    private void BreakConnect()
    {
        _aimBody.WakeUp();
        _rope.positionCount = 0;
        StopCoroutine(_ropeRendering);
    }

    private void OnJointBreak(float breakForce)
    {
        BreakConnect();
    }
}
