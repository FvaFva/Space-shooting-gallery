using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    private const float Speed = 5f;

    [Header("Interactable settings")]
    [SerializeField] private string _name;
    [SerializeField] private bool _customPickedView;
    [SerializeField] private float _dropPowerMultiplier = 100;

    private Rigidbody _body;
    private Collider _collider;

    public string Name => _name;
    public bool CustomPickedView => _customPickedView;
    public bool IsTaken {  get; private set; }

    public event Action ChangedState;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody tempBody) == false)
            gameObject.SetActive(false);

        if (TryGetComponent(out Collider tempCollider) == false)
            gameObject.SetActive(false);

        _collider = tempCollider;
        _body = tempBody;

        ValidateLoad();
    }

    public void Take(Transform hand)
    {
        TakeImpact(hand);
        _body.useGravity = false;
        _body.isKinematic = true;
        _collider.enabled = false;
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
        transform.SetParent(hand);
        transform.localRotation = hand.transform.localRotation;
        IsTaken = true;
        ChangedState?.Invoke();
        StartCoroutine(MovingToHand());
    }

    public void Drop(Vector3 forward)
    {
        _body.isKinematic = false;
        DropImpact(forward);
        _body.AddForce(forward * Speed * _dropPowerMultiplier);
        transform.SetParent(null);
        IsTaken = false;
        ChangedState?.Invoke();
        _collider.enabled = true;
        _body.useGravity = true;
    }

    public abstract void Interact();
    protected abstract void ValidateLoad();
    protected abstract void TakeImpact(Transform hand);
    protected abstract void DropImpact(Vector3 forward);

    private IEnumerator MovingToHand()
    {
        yield return null;

        while(transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Speed * Time.deltaTime);
            yield return null;
        }
    }
}