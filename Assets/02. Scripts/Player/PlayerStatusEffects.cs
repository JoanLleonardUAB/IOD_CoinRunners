using System.Collections;
using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    [SerializeField]
    private float baseSpeed = 5f;
    public float CurrentSpeed { get; private set; }
    public bool ControlsInverted { get; private set; }

    private void Awake()
    {
        CurrentSpeed = baseSpeed;
        ControlsInverted = false;
    }

    public IEnumerator Freeze(float duration)
    {
        float prev = CurrentSpeed;
        CurrentSpeed = 0;
        yield return new WaitForSeconds(duration);
        CurrentSpeed = prev;
    }

    public IEnumerator SpeedUp(float duration)
    {
        float prev = CurrentSpeed;
        CurrentSpeed = baseSpeed * 2f;
        yield return new WaitForSeconds(duration);
        CurrentSpeed = prev;
    }

    public IEnumerator SlowDown(float duration)
    {
        float prev = CurrentSpeed;
        CurrentSpeed = baseSpeed * 0.5f;
        yield return new WaitForSeconds(duration);
        CurrentSpeed = prev;
    }

    public IEnumerator InvertControls(float duration)
    {
        ControlsInverted = true;
        yield return new WaitForSeconds(duration);
        ControlsInverted = false;
    }
}
