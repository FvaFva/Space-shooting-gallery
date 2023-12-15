using UnityEngine;
using DG.Tweening;
using TMPro;
using Zenject;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
public class TemporaryInfo : MonoBehaviour
{
    private const float ShowTime = 2;
    private const float FadeTime = 3;

    [SerializeField] private TMP_Text _infoField;

    [Inject] private PlayerHand _hand;
    [Inject] private PlayerInputMap _inputMap;

    private CanvasGroup _canvasGroup;
    private Canvas _mainCanvas;
    private string _dropKey;

    private void Awake()
    {
        Canvas tempCanvas = null;
        gameObject.SetActive(TryGetComponent(out CanvasGroup tempGroup) && TryGetComponent(out tempCanvas) && _infoField != null);
        _canvasGroup = tempGroup;
        _mainCanvas = tempCanvas;
        CacheDropSymbol();
    }

    private void OnEnable()
    {
        _hand.ChangedItemInHand += OnChangedItem;
    }

    private void OnDisable()
    {
        _hand.ChangedItemInHand -= OnChangedItem;
    }

    public void OnChangedItem(Interactable item)
    {
        if (item != null)
            Show($"Press {_dropKey} to throw");
    }

    private void Show(string info)
    {
        _infoField.text = info;
        _canvasGroup.alpha = 1.0f;
        _mainCanvas.enabled = true;
        _canvasGroup.DOFade(0, FadeTime).SetDelay(ShowTime).OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        _infoField.text = "";
        _mainCanvas.enabled = false;
    }

    private void CacheDropSymbol()
    {
        IEnumerable<InputBinding> binds = _inputMap.KeyboardAndMouse.DropItem.bindings;

        if(binds == null || binds.Count() == 0)
            return;

        _dropKey = binds.First().path.ToUpper();
        int indexOfDevore = _dropKey.IndexOf("/");

        if (indexOfDevore != -1)
            _dropKey = _dropKey.Substring(indexOfDevore + 1);
    }
}
