using UnityEngine;
using System.Collections;
using Zenject;
using System;

[RequireComponent (typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private const float ReverseVelocityCoefficient = -3.5f;
    private const float VerticalMinAngle = -89f;
    private const float VerticalMaxAngle = 89f;
    private const float DelayTime = 0.01f;
    private const float FallShakeDuration = 0.3f;
    private const float FallShakeCoefficient = 7f;
    private const float MicroFallTime = 0.2f;
    private const float MicroVelocity = 0.01f;
    private const float StabilizerFactor = 90;

    [SerializeField] private Camera _camera;

    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _strafeSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _gravityFactor;
    [SerializeField] private GroundChecker _groundChecker;

    [Header("Rotate settings")]
    [SerializeField] private float _horizontalTurnSensitivity;
    [SerializeField] private float _verticalTurnSensitivity;

    [Header("Sounds")]
    [SerializeField] private AudioSource _fallSound;

    [Inject] private PlayerInputMap _input;
    [Inject] private CameraShaker _shaker;

    private Rigidbody _body;
    private Coroutine _moving;
    private WaitForSeconds _delay;

    private float _cameraAngle = 0;
    private float _flyTime = 0;
    private Vector3 _gravity;
    private Func<Vector3> _currentForceProcessor;
    private bool _canJump;

    private void Awake()
    {
        if(TryGetComponent(out Rigidbody controller) == false)
            gameObject.SetActive(false);

        _body = controller;
        _gravity = Physics.gravity;
        _delay = new WaitForSeconds(DelayTime);
        _cameraAngle = _camera.transform.localEulerAngles.x;
        _currentForceProcessor = CalculateForceVertical;
    }

    private void OnEnable()
    {
        StartMoving();
    }

    private void OnDisable()
    {
        StopMoving();
    }

    public void StartMoving()
    {
        StopMoving();
        _input.KeyboardAndMouse.Jump.performed += ctx => Jump();
        _groundChecker.Changed += OnGroundChange;
        _moving = StartCoroutine(Moving());
    }

    public void StopMoving()
    {
        _groundChecker.Changed -= OnGroundChange;
        _input.KeyboardAndMouse.Jump.performed -= ctx => Jump();

        if (_moving != null)
        {
            StopCoroutine(Moving());
            _moving = null;
        }
    }

    private IEnumerator Moving()
    {
        yield return null;

        while (true)
        {
            Rotate();

            _body.AddForce(_currentForceProcessor() * Time.deltaTime * StabilizerFactor);

            yield return _delay;
        }
    }

    private Vector3 CalculateForceHorizontal()
    {
        Vector2 inputForward = _input.KeyboardAndMouse.MovementVector.ReadValue<Vector2>().normalized;
        Vector3 velocity = HorizontalVelocity();

        if(inputForward.magnitude == 0)
        {
            if(velocity.magnitude > MicroVelocity) 
                return velocity * ReverseVelocityCoefficient;
            else 
                return Vector3.zero;
        }

        Vector3 cameraForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
        Vector3 inputForce = cameraForward * inputForward.y * _speed + cameraRight * inputForward.x * _strafeSpeed;
        inputForce += inputForce - velocity;
        return inputForce;
    }

    private Vector3 CalculateForceVertical()
    {
        _flyTime += Time.deltaTime;
        return HorizontalVelocity() + _gravity * _gravityFactor;
    }

    private void Rotate()
    {
        Vector2 rotateForward = _input.KeyboardAndMouse.Look.ReadValue<Vector2>();

        if (rotateForward.y != 0)
        {
            _cameraAngle -= rotateForward.y * _verticalTurnSensitivity;
            _cameraAngle = Mathf.Clamp(_cameraAngle, VerticalMinAngle, VerticalMaxAngle);
            _camera.transform.localEulerAngles = Vector3.right * _cameraAngle;
        }

        transform.Rotate(Vector3.up * _horizontalTurnSensitivity * rotateForward.x * Time.deltaTime * StabilizerFactor);
    }

    private void Jump()
    {
        if(_canJump)
            _body.AddForce(HorizontalVelocity() + _jumpSpeed * Vector3.up);
    }

    private Vector3 HorizontalVelocity()
    {
        Vector3 horizontalVelocity = _body.velocity;
        horizontalVelocity.y = 0;
        return horizontalVelocity;
    }

    private void OnGroundChange(bool isGrounded)
    {
        _canJump = isGrounded;

        if (isGrounded)
        {
            _currentForceProcessor = CalculateForceHorizontal;
            
            if(_flyTime > MicroFallTime)
            {
                _shaker.MakeShake(_flyTime * FallShakeCoefficient, FallShakeDuration);
                _fallSound?.Play();
            }

            _flyTime = 0;
        }
        else
        {
            _currentForceProcessor = CalculateForceVertical;
        }
    }
}
