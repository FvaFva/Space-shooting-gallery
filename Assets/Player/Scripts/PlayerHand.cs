using System;
using UnityEngine;
using Zenject;

public class PlayerHand : MonoBehaviour
{
    private Transform _position;
    private Interactable _current;

    [SerializeField] private AudioSource _takeSound;
    [SerializeField] private AudioSource _dropSound;
    [SerializeField] private Transform _handPosition;

    [Inject] private PlayerInputMap _playerInput;
    [Inject] private ScannerInteractable _scanner;

    public event Action<Interactable> ChangedItemInHand;

    private void Awake()
    {
        _position = transform;
    }

    private void OnEnable()
    {
        _playerInput.KeyboardAndMouse.ActivateItem.performed += ctx => ActivateItem();
        _playerInput.KeyboardAndMouse.DropItem.performed += ctx => DropItem();
        _playerInput.KeyboardAndMouse.TakeItem.performed += ctx => TakeItem();
    }

    private void OnDisable()
    {
        _playerInput.KeyboardAndMouse.ActivateItem.performed -= ctx => ActivateItem();
        _playerInput.KeyboardAndMouse.DropItem.performed -= ctx => DropItem();
        _playerInput.KeyboardAndMouse.TakeItem.performed -= ctx => TakeItem();
    }

    private void TakeItem()
    {
        if (_scanner.CurrentItem == null)
            return;

        _current = _scanner.CurrentItem;
        _scanner.StopSearching();

        if(_current.CustomPickedView)
            _current.Take(_position);
        else
            _current.Take(_handPosition);

        ChangedItemInHand?.Invoke(_current);
        _takeSound.Play();
    }

    private void ActivateItem()
    {
        _current?.Interact();
    }

    private void DropItem()
    {
        _current?.Drop(_position.forward);
        _current = null;
        _scanner.StartSearching();
        ChangedItemInHand?.Invoke(_current);
        _dropSound.Play();
    }
}
