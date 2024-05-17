using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleObtainer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sickleAudio;
    [SerializeField]
    private float _sickleDuration;

    private bool _hasSickle = false;
    private float _sickleObtainedOn = 0f;

    public bool HasSickle => _hasSickle;

    public Action OnSickleObtained;
    public Action OnSickleLost;

    private void OnTriggerEnter(Collider other)
    {
        // If the trigger is of type coin
        if (other.gameObject.CompareTag("Sickle"))
        {
            // Add a coin and call everyone who listens to the coin being obtained
            _hasSickle = true;
            _sickleObtainedOn = Time.time;
            OnSickleObtained?.Invoke();
            var sickleObject = GetComponentInChildren<SickleObject>(true);
            if (sickleObject != null) sickleObject.gameObject.SetActive(true);
            // Then return the coin to the pool
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            coinBehaviour.CoinGot();
            if (_sickleAudio != null) _sickleAudio.Play();
        }
    }

    private void Update()
    {
        if ( _hasSickle && Time.time > _sickleObtainedOn + _sickleDuration)
        {
            _hasSickle = false;
            var sickleObject = GetComponentInChildren<SickleObject>(true);
            if (sickleObject != null) sickleObject.gameObject.SetActive(false);
            OnSickleLost?.Invoke();
        }
    }
}
