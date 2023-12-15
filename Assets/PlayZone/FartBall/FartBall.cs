using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FartBall : Interactable
{
    private int _play;
    private Animator _animator;

    [SerializeField] private AudioSource _fart;
    [SerializeField] private AudioSource _dropFart;
    [SerializeField] private DamageReactive _damageTaker;

    private void OnEnable()
    {
        _damageTaker.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _damageTaker.Damaged -= OnDamaged;
    }

    public override void Interact()
    {
        _fart.Play();
        _animator.SetTrigger(_play);
    }

    protected override void ValidateLoad()
    {
        gameObject.SetActive(TryGetComponent(out Animator temp));
        _play = Animator.StringToHash("Play");
        _animator = temp;
    }

    protected override void DropImpact(Vector3 forward)
    {
        PlayBigFart();
    }

    protected override void TakeImpact(Transform hand)
    {
    }

    private void OnDamaged(float damage)
    {
        PlayBigFart();
    }

    private void PlayBigFart()
    {
        _dropFart.Play();
        _animator.SetTrigger(_play);
    }
}
