public class SpeedUpEffect : IPowerEffect
{
    public void Apply(PlayerStatusEffects target, float duration)
    {
        target.StartCoroutine(target.SpeedUp(duration));
    }
}
