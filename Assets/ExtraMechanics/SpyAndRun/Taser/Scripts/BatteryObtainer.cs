using System;
using UnityEngine;

public class BatteryObtainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The audio source with the battery sound")]
    private AudioSource _batteryAudio;
    [SerializeField]
    private int _usesPerBattery;

    public Action<int> OnBatteryObtained;
    private int _batteries = 0;

    public int Batteries => _batteries;

    // When the player collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the trigger is of type coin
        if (other.gameObject.CompareTag("Battery"))
        {
            // Add a coin and call everyone who listens to the coin being obtained
            _batteries += _usesPerBattery;
            OnBatteryObtained?.Invoke(_batteries);
            // Then return the coin to the pool
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            coinBehaviour.CoinGot();
            if (_batteryAudio != null) _batteryAudio.Play();
        }
    }

    public bool HasBatteries()
    {
        return _batteries > 0;
    }

    public void ConsumeBattery()
    {
        _batteries--;
        OnBatteryObtained?.Invoke(_batteries);
    }
}
