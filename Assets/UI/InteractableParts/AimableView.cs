public class AimableView : BaseItemView
{
    public override void CheckInteractableView(Interactable picked)
    {
        CheckItemComponent<IAimable>(picked);
    }
}