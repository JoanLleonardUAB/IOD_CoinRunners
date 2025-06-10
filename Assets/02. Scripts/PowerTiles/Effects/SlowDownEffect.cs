public class SlowDownEffect : IPowerEffect
{
    public void Apply(PlayerStatusEffects target, float duration)
    {
        target.StartCoroutine(target.SlowDown(duration));
    }
}
