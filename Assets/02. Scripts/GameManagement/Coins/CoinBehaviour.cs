using System;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    private int _value;

    [SerializeField]
    private Vector2 _minMaxTimeToDisappear;

    [SerializeField]
    private Animator _animator;

    public int Value => _value;

    public Action<CoinBehaviour> OnCoinGot;

    Transform _spawnReference;

    public Transform SpawnReference => _spawnReference;
    private float _time;
    private bool _isUp = false;

    private void OnEnable()
    {
        _time = UnityEngine.Random.Range(_minMaxTimeToDisappear.x, _minMaxTimeToDisappear.y);
        _isUp = true;
        _animator.SetTrigger("Appear");
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            Hide();
        }
    }

    public void SetSpawnReference(Transform reference)
    {
        _spawnReference = reference;
    }

    private void CoinGot()
    {
        OnCoinGot?.Invoke(this);
    }

    private void Hide()
    {
        _isUp = false;
        _animator.SetTrigger("Hide");
        CoinGot();
    }

    public bool Die()
    {
        if (!_isUp) return false;
        _isUp = false;
        _animator.SetTrigger("Die");
        CoinGot();
        return true;
    }
}
