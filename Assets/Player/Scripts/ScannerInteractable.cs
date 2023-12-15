using System;
using System.Collections;
using UnityEngine;

public class ScannerInteractable : MonoBehaviour
{
    private const float SearchingDelay = 0.2f;
    private const float CheckDistance = 1.7f;

    private WaitForSeconds _delay;
    private Coroutine _searching;
    private bool _isActive;

    public Interactable CurrentItem {  get; private set; }

    public event Action<Interactable> FoundItem;
    public event Action LostItem;

    private void Awake()
    {
        _delay = new WaitForSeconds(SearchingDelay);
        _isActive = true;
    }

    private void OnEnable()
    {
         if (_isActive)
            StartSearching();
    }

    private void OnDisable()
    {
        if (_searching != null)
            StopCoroutine(_searching);
    }

    public void StartSearching()
    {
        _isActive = true;

        if (_searching == null)
            _searching = StartCoroutine(Search());
    }

    public void StopSearching()
    {
        _isActive = false;
        ChangeCurrentItem(null);

        if (_searching != null)
            StopCoroutine(_searching);

        _searching = null;
    }

    private IEnumerator Search()
    {
        yield return null;
        RaycastHit hitInfo;
        Interactable foundItem;
        CurrentItem = null;

        while (_isActive)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, CheckDistance) &&
                TryGetItemFromCollision(hitInfo, out foundItem))
            {
                if(foundItem != CurrentItem)
                    ChangeCurrentItem(foundItem);
            }
            else if(CurrentItem != null)
            {
                ChangeCurrentItem(null);
            }
        
            yield return _delay;
        }

        _searching = null;
    }

    private void ChangeCurrentItem(Interactable newItem)
    {
        if(CurrentItem != null)
        {
            CurrentItem.ChangedState -= OnCurrentChangedState;
            LostItem?.Invoke();
        }

        CurrentItem = newItem;

        if (CurrentItem != null)
        {
            CurrentItem.ChangedState += OnCurrentChangedState;
            FoundItem?.Invoke(CurrentItem);
        }
    }

    private void OnCurrentChangedState()
    {
        if(CurrentItem.IsTaken == false)
        {
            ChangeCurrentItem(null);
        }
    }

    private bool TryGetItemFromCollision(RaycastHit collision, out Interactable interactable)
    {
        interactable = null;

        if (collision.collider == null)
            return false;

        interactable = collision.collider.GetComponentInParent<Interactable>();

        return interactable != null && interactable.IsTaken == false;
    }
}
