using System;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public Action<CoinBehaviour> OnCoinGot;

    Transform _spawnReference;

    public Transform SpawnReference => _spawnReference;

    // Add coin ID
    [SerializeField]
    private int _coinId;
    public int CoinId => _coinId;

    public void SetSpawnReference(Transform reference)
    {
        _spawnReference = reference;
    }

    public void CoinGot()
    {
        OnCoinGot?.Invoke(this);
    }
}
