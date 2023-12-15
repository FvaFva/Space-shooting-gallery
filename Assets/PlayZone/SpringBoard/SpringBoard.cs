using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class SpringBoard : MonoBehaviour
{
    [SerializeField] private float _secondsRecharge;
    [SerializeField] private HingeJoint _hinge;
    [SerializeField] private float _jumpForce;
    [SerializeField] private AudioSource _effect;

    private WaitForSeconds _sleep;
    private bool _isReady = true;

    private void Awake()
    {
        enabled = TryGetComponent(out HingeJoint hinge);

        _hinge = hinge;
        _sleep = new WaitForSeconds(_secondsRecharge);
    }

    private void OnDisable()
    {
        Deactivate();
    }

    private void Activate()
    {
        ConnectToAnchor();
        _effect.Play();
        StartCoroutine(Recharge());
    }

    private IEnumerator Recharge()
    {
        yield return _sleep;
        Deactivate();
    }

    private void ConnectToAnchor()
    {
        _isReady = false;
        _hinge.useSpring = true;
    }

    private void Deactivate()
    {
        _hinge.useSpring = false;
        _isReady = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isReady)
        {
            Rigidbody body = collision.rigidbody;

            if (body?.isKinematic == false)
            {
                body.AddForceAtPosition(Vector3.up * _jumpForce, collision.GetContact(0).point);
                Activate();
            }
        }
    }
}
