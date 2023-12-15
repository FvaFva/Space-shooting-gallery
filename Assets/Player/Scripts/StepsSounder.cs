using System.Collections;
using UnityEngine;

public class StepsSounder : MonoBehaviour
{
    private const float Delay = 0.015f;

    [SerializeField, Range (0.01f, 2f)] private float _distanceToPlay;
    [SerializeField] private AudioSource[] _audioSource;
    [SerializeField] private GroundChecker _groundChecker;

    private WaitForSeconds _delay;
    private Transform _transform;
    private Coroutine _sounder;
    private int _countElements;

    private void Awake()
    {
        _transform = transform;
        _delay = new WaitForSeconds(Delay);
        _countElements = _audioSource.Length;
        enabled = _countElements > 0 && _groundChecker != null;
    }

    private void OnEnable()
    {
        _groundChecker.Changed += OnStateChange;
    }

    private void OnDisable()
    {
        _groundChecker.Changed -= OnStateChange;
    }

    private void OnStateChange(bool isGrounded)
    {
        if (_sounder != null)
            StopCoroutine(_sounder);

        if (isGrounded)
            _sounder = StartCoroutine(Sounding());
    }

    private IEnumerator Sounding()
    {
        Vector3 oldPosition = _transform.position;
        float currentDistance = 0;
        yield return null;

        while(true)
        {
            currentDistance += Mathf.Abs((_transform.position - oldPosition).magnitude);
            oldPosition = _transform.position;

            if(currentDistance > _distanceToPlay)
            {
                _audioSource[Random.Range(0, _countElements)].Play();
                currentDistance = 0;
            }

            yield return _delay;
        }
    }
}
