using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TaserShot : MonoBehaviour
{
    [SerializeField]
    private BatteryObtainer _batteryObtainer;
    [SerializeField]
    private float _shotDelay;
    [SerializeField]
    private GameObject _taserPrefab;
    [SerializeField]
    private float _forwardOffset = 1f;
    [SerializeField]
    [Tooltip("The audio source with the battery sound")]
    private AudioSource _taserAudio;
    [SerializeField]
    private Movement _movement;
    [SerializeField]
    private float _shotStopTime;

    private float _lastShot = 0f;

    public void Shot(InputAction.CallbackContext ctx)
    {
        if (ctx.started && Time.time - _lastShot > _shotDelay && _batteryObtainer.HasBatteries())
        {
            _batteryObtainer.ConsumeBattery();
            _lastShot = Time.time;
            DoShot();
        }
    }

    private void DoShot()
    {
        Debug.Log("Taser shot!");
        var go = Instantiate(_taserPrefab);
        go.transform.position = transform.position + transform.forward * _forwardOffset;
        var euler = transform.eulerAngles;
        euler.x = 0;
        euler.z = 0f;
        go.transform.eulerAngles = euler;
        if (_taserAudio != null) _taserAudio.Play();
        var animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetTrigger("Shot");
        if (_shotStopTime > 0f)
        {
            _movement.Stop(_shotStopTime);
        }
    }
}
