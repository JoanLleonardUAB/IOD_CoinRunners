public class InvertControlsEffect : IPowerEffect
{
    public void Apply(PlayerStatusEffects target, float duration)
    {
        target.StartCoroutine(target.InvertControls(duration));
    }
}
