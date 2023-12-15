using TMPro;
using UnityEngine;
using Zenject;

public class InteractableViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private Canvas _viewer;

    [Inject] private ScannerInteractable _scanner;

    private void OnEnable()
    {
        _scanner.FoundItem += ViewInteractable;
        _scanner.LostItem += HideInteractable;
    }

    private void OnDisable()
    {
        _scanner.FoundItem -= ViewInteractable;
        _scanner.LostItem -= HideInteractable;
    }

    private void ViewInteractable(Interactable item)
    {
        _itemName.text = item.Name;
        _viewer.enabled = true;
    }

    private void HideInteractable()
    {
        _viewer.enabled = false;
    }
}
