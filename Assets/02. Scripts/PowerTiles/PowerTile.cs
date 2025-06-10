using UnityEngine;

public enum PowerEffectType
{
    Freeze,
    SpeedUp,
    SlowDown,
    InvertControls,
}

public static class PowerEffectFactory
{
    public static IPowerEffect Create(PowerEffectType type)
    {
        switch (type)
        {
            case PowerEffectType.Freeze:
                return new FreezeEffect();
            case PowerEffectType.SpeedUp:
                return new SpeedUpEffect();
            case PowerEffectType.SlowDown:
                return new SlowDownEffect();
            case PowerEffectType.InvertControls:
                return new InvertControlsEffect();
            default:
                return null;
        }
    }
}

public class PowerTile : MonoBehaviour
{
    public Color emissionColor = Color.white;
    public float effectDuration = 2f;
    public PowerEffectType effectType;

    Material mat;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        SetEmission(false);
    }

    public void Activate()
    {
        SetEmission(true);
    }

    public void Deactivate()
    {
        SetEmission(false);
    }

    void SetEmission(bool state)
    {
        if (mat != null)
        {
            if (state)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", emissionColor);
            }
            else
            {
                mat.DisableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var statusEffects = other.GetComponent<PlayerStatusEffects>();
        IPowerEffect effect = PowerEffectFactory.Create(effectType);
        if (statusEffects != null && effect != null)
        {
            effect.Apply(statusEffects, effectDuration);
        }
    }
}
