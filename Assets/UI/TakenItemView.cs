using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TakenItemView : MonoBehaviour
{
    [SerializeField] private List<BaseItemView> _viewers;

    [Inject] private readonly PlayerHand _hand;

    private void OnEnable()
    {
        _hand.ChangedItemInHand += ChangedCurrent;
    }

    private void OnDisable()
    {
        _hand.ChangedItemInHand -= ChangedCurrent;
    }

    public void ChangedCurrent(Interactable item)
    {
        foreach (BaseItemView view in _viewers)
            view.CheckInteractableView(item);
    }
}
