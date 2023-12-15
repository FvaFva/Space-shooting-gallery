using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private const float RecoilRandom = 0.5f;

    [Header("Noise")]
    [SerializeField] private float _perlinNoiseScale = 1f;
    [SerializeField] private AnimationCurve _perlinNoiseAmplitudeCurve;

    [Header("Recoil")]
    [SerializeField] private float _tension = 150f;
    [SerializeField] private float _damping = 7f;

    private Transform _shakable;
    private Vector3 _shakeAngles = new Vector3();
    private Vector3 _recoilAngles = new Vector3();
    private Vector3 _recoilVelocity = new Vector3();

    private float _amplitude = 0;
    private float _shakeTimer = 0;
    private float _duration = 0;

    private void Awake()
    {
        _shakable = transform;
    }

    private void Update()
    {
        UpdateRecoil();
        UpdateShake();

        _shakable.localEulerAngles = _shakeAngles + _recoilAngles;
    }

    public void MakeShake(float amplitude, float duration)
    {
        _amplitude = amplitude;
        _duration = duration;
        _shakeTimer = 1;
    }

    public void MakeRecoil(float impulse)
    {
        Vector3 horizontalRecoilImpact = -Vector3.right * Random.Range(impulse * RecoilRandom, impulse);
        Vector3 verticalRecoilImpact = Vector3.up * Random.Range(impulse * RecoilRandom, -impulse * RecoilRandom);
        _recoilVelocity += horizontalRecoilImpact + verticalRecoilImpact;
    }

    private void UpdateRecoil()
    {
        float delta = Time.deltaTime;
        _recoilAngles += _recoilVelocity * delta;
        _recoilVelocity -= _recoilAngles * delta * _tension;
        _recoilVelocity = Vector3.Lerp(_recoilVelocity, Vector3.zero, _damping * delta);
    }

    private void UpdateShake()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime / _duration;

            float time = Time.time * _perlinNoiseScale;
            _shakeAngles.x = Mathf.PerlinNoise(time, 0);
            _shakeAngles.y = Mathf.PerlinNoise(0, time);
            _shakeAngles.z = Mathf.PerlinNoise(time, time);

            _shakeAngles *= _amplitude;
            _shakeAngles *= _perlinNoiseAmplitudeCurve.Evaluate(Mathf.Clamp01(1 - _shakeTimer));
        }
    }
}