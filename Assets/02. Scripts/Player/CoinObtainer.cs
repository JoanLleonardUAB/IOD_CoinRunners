using System;
using UnityEngine;

public class CoinObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;
    [SerializeField]
    [Tooltip("The audio source with the coin lost sound")]
    private AudioSource _coinLossAudio;
    [SerializeField]
    private ParticleSystem _coinLossEffect;

    public Action<int> OnCoinObtained;
    private int _coins = 0;

    public int Coins => _coins;

    // When the player collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the trigger is of type coin
        if (other.gameObject.CompareTag("Coin"))
        {
            // Add a coin and call everyone who listens to the coin being obtained
            _coins++;
            OnCoinObtained?.Invoke(_coins);
            // Then return the coin to the pool
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            coinBehaviour.CoinGot();
            if (_coinAudio != null) _coinAudio.Play();
        }
    }

    public void LoseCoins(int amount)
    {
        if (_coins == 0) return;
        _coins -= amount;
        _coins = System.Math.Max(_coins, 0);
        if (_coinLossAudio != null) _coinLossAudio.Play();
        OnCoinObtained?.Invoke(_coins);
        if (_coinLossEffect != null) _coinLossEffect.Play();
    }
}
