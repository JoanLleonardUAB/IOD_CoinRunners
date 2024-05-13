using System;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public enum MaterialType
    {
        MaterialA = 0,
        MaterialB = 1,
        MaterialC = 2
    }
    public Action<CoinBehaviour> OnCoinGot;

    [SerializeField]
    private MaterialType _materialType;

    public MaterialType Material => _materialType;

    Transform _spawnReference;

    public Transform SpawnReference => _spawnReference;

    public void SetSpawnReference(Transform reference)
    {
        _spawnReference = reference;
    }

    public void CoinGot()
    {
        OnCoinGot?.Invoke(this);
    }
}
