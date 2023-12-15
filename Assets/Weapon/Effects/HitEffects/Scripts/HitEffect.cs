using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private GameObject _bulletHole;
    [SerializeField] private AudioSource _hitSound;

    public bool IsFree => !_effect.isPlaying;

    private void Awake()
    {
        _effect.Stop();
    }

    public void Show(Vector3 position, Vector3 normal, Transform newParent)
    {
        transform.position = position;
        transform.rotation = Quaternion.FromToRotation(transform.forward, normal);
        transform.SetParent(newParent);

        _effect.Play();
        _hitSound?.Play();
    }
}
