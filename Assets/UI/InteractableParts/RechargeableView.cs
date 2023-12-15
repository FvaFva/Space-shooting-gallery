using UnityEngine;
using UnityEngine.UI;

public class RechargeableView : BaseItemView
{
    [SerializeField] private Slider _recharge;
    private IRechargeable _current;

    public override void CheckInteractableView(Interactable picked)
    {
        if (_current != null)
            _current.RechargeProgressChanged -= ChangeRecharge;
        
        _current = null;

        if(CheckItemComponent<IRechargeable>(picked))
        {
            _current = (IRechargeable)picked;
            _current.RechargeProgressChanged += ChangeRecharge;
        }

        _recharge.value = 1;
    }

    private void ChangeRecharge(float value)
    {
        _recharge.value = value;
    }
}
