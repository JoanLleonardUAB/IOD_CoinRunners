public class FreezeEffect : IPowerEffect
{
    public void Apply(PlayerStatusEffects target, float duration)
    {
        target.StartCoroutine(target.Freeze(duration));
    }
}
