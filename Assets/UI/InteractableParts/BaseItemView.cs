using UnityEngine;

public abstract class BaseItemView: MonoBehaviour
{
    [SerializeField] private Canvas _main;

    protected bool CheckItemComponent<T>(Interactable picked)
    {
        bool isCorrect = picked is T;
        _main.enabled = isCorrect;
        return isCorrect;
    }

    public abstract void CheckInteractableView(Interactable picked);
}
