using System;
using UnityEngine;

public class CoinObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;

    public Action<int> OnCoinObtained;
    private int _coins = 0;

    public int Coins => _coins;

    // When the player hits the mole with the mallet
    public void ObtainCoin(CoinBehaviour coin)
    {
        if (coin.Die())
        {
            // Add a coin and call everyone who listens to the coin being obtained
            _coins += coin.Value;
            OnCoinObtained?.Invoke(_coins);
            // Then return the coin to the pool
            if (_coinAudio != null) _coinAudio.Play();
        }
    }
}
