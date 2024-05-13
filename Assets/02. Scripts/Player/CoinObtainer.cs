using System;
using UnityEngine;

public class CoinObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;

    public Action<int[], CoinBehaviour.MaterialType> OnCoinObtained;
    private int[] _coins = new int[3];

    public int[] Coins => _coins;

    // When the player collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the trigger is of type coin
        if (other.gameObject.CompareTag("Coin"))
        {
            // Then return the coin to the pool
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            coinBehaviour.CoinGot();
            // Add a coin and call everyone who listens to the coin being obtained
            _coins[(int)coinBehaviour.Material]++;
            OnCoinObtained?.Invoke(_coins, coinBehaviour.Material);
            if (_coinAudio != null) _coinAudio.Play();
        }
    }
}
