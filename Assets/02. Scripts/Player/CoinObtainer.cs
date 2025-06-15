using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;
    private int differentCoins = 4;

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

    [Header("Score Marker")]
    public Transform scoreMarker; // Set dynamically on player join
    public GameObject[] coinMarkerPrefabs; // Assign each coin type prefab in the inspector

    private List<GameObject> _spawnedMarkers = new List<GameObject>();

    void Start()
    {
        differentCoins = coinMarkerPrefabs.Length;
    }

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
        SpawnCoinMarkers();
    }

    public void SetScoreMarker(Transform marker)
    {
        scoreMarker = marker;
    }

    private void SpawnCoinMarkers()
    {
        // Clear previous markers
        foreach (var go in _spawnedMarkers)
            Destroy(go);
        _spawnedMarkers.Clear();

        if (scoreMarker == null || coinMarkerPrefabs == null)
            return;

        for (int i = 0; i < _deliveryOrder.Count; i++)
        {
            int coinId = _deliveryOrder[i];
            // coinId assumed to be 1-based; adjust to 0-based index
            int prefabIndex = Mathf.Clamp(coinId - 1, 0, coinMarkerPrefabs.Length - 1);
            var markerObj = Instantiate(coinMarkerPrefabs[prefabIndex], scoreMarker);
            markerObj.transform.localPosition = Vector3.down * i * 1.2f; // Adjust spacing as needed

            // Optionally highlight the first marker
            var markerScript = markerObj.GetComponent<CoinMarker>();
            if (markerScript != null)
                markerScript.SetHighlight(i == _deliveryProgress);

            _spawnedMarkers.Add(markerObj);
        }
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

                // Si ya entregó todas las monedas, termina el temporizador
                if (_deliveryProgress >= _deliveryOrder.Count)
                {
                    GameTimer.instance.ForceEnd();
                }
            }
            else
            {
                // Incorrect coin delivered, reset progress
                Debug.Log("Incorrect coin delivered! Progress reset.");
                ResetProgress();
            }
        }

        UpdateCoinMarkerHighlight();
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

    private void UpdateCoinMarkerHighlight()
    {
        for (int i = 0; i < _spawnedMarkers.Count; i++)
        {
            var markerScript = _spawnedMarkers[i].GetComponent<CoinMarker>();
            if (markerScript != null)
                markerScript.SetHighlight(i == _deliveryProgress);
        }
    }
}
