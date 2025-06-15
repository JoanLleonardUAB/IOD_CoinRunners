using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;

    [Header("Coin Order Settings")]
    [Tooltip("How many different coin IDs exist?")]
    public int differentCoins = 4;

    [Tooltip("How many coins must be delivered in order?")]
    public int maxCoinsToGet = 5;

    public Action<int> OnCoinObtained;
    private int _coins = 0;
    public int Coins => _coins;

    // Store the currently held coin
    private CoinBehaviour _heldCoin = null;

    // The randomized delivery order (list of coin IDs)
    private List<int> _deliveryOrder = new List<int>();
    private int _deliveryProgress = 0; // Index in delivery order

    // Llama esto cuando el jugador entra
    public void GenerateDeliveryOrder()
    {
        _deliveryOrder.Clear();
        for (int i = 0; i < maxCoinsToGet; i++)
        {
            int randomId = UnityEngine.Random.Range(1, differentCoins + 1); // IDs entre 1 y differentCoins
            _deliveryOrder.Add(randomId);
        }
        _deliveryProgress = 0;
        Debug.Log("Orden generado: " + string.Join(", ", _deliveryOrder));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pick up a coin if not already holding one
        if (other.gameObject.CompareTag("Coin") && _heldCoin == null)
        {
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            _heldCoin = coinBehaviour;
            coinBehaviour.gameObject.SetActive(false); // Hide the coin until delivered
            if (_coinAudio != null)
                _coinAudio.Play();
        }
        // Deliver the coin if in delivery zone
        else if (other.gameObject.CompareTag("DeliveryZone") && _heldCoin != null)
        {
            if (_deliveryOrder.Count == 0)
            {
                Debug.LogWarning("Delivery order not set!");
                return;
            }

            int expectedCoinId = _deliveryOrder[_deliveryProgress];
            if (_heldCoin.CoinId == expectedCoinId)
            {
                // Correct coin delivered
                _coins++;
                OnCoinObtained?.Invoke(_coins);
                _heldCoin.CoinGot(); // Return coin to pool
                _heldCoin = null;
                _deliveryProgress++;
                if (_coinAudio != null)
                    _coinAudio.Play();
                // Puedes checar si ya terminó aquí
            }
            else
            {
                // Incorrect coin delivered, reset progress
                Debug.Log("Incorrect coin delivered! Progress reset.");
                ResetProgress();
            }
        }
    }

    private void ResetProgress()
    {
        _deliveryProgress = 0;
        if (_heldCoin != null)
        {
            _heldCoin.gameObject.SetActive(true); // Put coin back in the field
            _heldCoin = null;
        }
        _coins = 0;
        OnCoinObtained?.Invoke(_coins);
    }
}
