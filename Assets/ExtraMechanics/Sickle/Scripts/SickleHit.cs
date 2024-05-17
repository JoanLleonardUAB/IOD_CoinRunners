using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SickleHit : MonoBehaviour
{
    [SerializeField]
    private SickleObtainer _obtainer;
    [SerializeField]
    private float _hitDelay;
    [SerializeField]
    [Range(0f, 1f)]
    private float _minAngle;
    [SerializeField]
    private float _maxDistance;
    [SerializeField]
    private float _force;
    [SerializeField]
    private int _lossAmount;

    private float _lastHitDone;

    public void Hit(InputAction.CallbackContext ctx)
    {
        if (ctx.started && _obtainer.HasSickle && Time.time > _lastHitDone + _hitDelay)
        {
            DoHit();
        }
    }

    private void DoHit()
    {
        var animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetTrigger("Sickle");

        var hits = FindObjectsOfType<SickleHitReceiver>();
        foreach(var hit in hits)
        {
            var vec = hit.transform.position - transform.position;
            var distance = vec.magnitude;
            var dir = vec.normalized;
            var dot = Vector3.Dot(transform.forward, dir);

            if (distance <= _maxDistance && dot > _minAngle)
            {
                hit.GotHit(_lossAmount, dir*_force);
            }
        }
    }
}
