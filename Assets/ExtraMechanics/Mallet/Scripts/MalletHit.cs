using UnityEngine;
using UnityEngine.InputSystem;

public class MalletHit : MonoBehaviour
{
    [SerializeField]
    private float _hitDelay = 1f;
    [SerializeField]
    private Transform _hitOrigin;
    [SerializeField]
    private float _hitRadius;
    [SerializeField]
    private CoinObtainer _obtainer;
    [SerializeField]
    private GameObject _malletHitEffectPrefab;
    [SerializeField]
    private float _malletHitEffectDuration;

    private float _lastHitTime = 0f;

    public void Hit(InputAction.CallbackContext ctx)
    {
        if (ctx.started && Time.time - _lastHitTime >= _hitDelay)
        {
            DoHit();
            _lastHitTime = Time.time;
        }
    }

    private void DoHit()
    {
        Debug.Log("Hitting with mallet!");

        var go = Instantiate(_malletHitEffectPrefab, _hitOrigin.position, Quaternion.identity);
        Destroy(go, _malletHitEffectDuration);

        var animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Mallet");
        var hits = Physics.OverlapSphere(_hitOrigin.position, _hitRadius);

        foreach(var hit in hits)
        {
            if (hit.gameObject.CompareTag("Coin"))
            {
                CoinBehaviour coin = hit.GetComponent<CoinBehaviour>();
                _obtainer.ObtainCoin(coin);
            }
        }
    }
}
