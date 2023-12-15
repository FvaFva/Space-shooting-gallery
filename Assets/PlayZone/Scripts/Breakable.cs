public class Breakable : BaseHealth
{
    protected override void Die()
    {
        gameObject.SetActive(false);
    }
}