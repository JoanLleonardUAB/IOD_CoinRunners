using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseConstruction : MonoBehaviour
{
    [SerializeField] [Range(0, 3)]
    private int _playerSlot;

    [SerializeField]
    private GameObject[] _baseSteps;

    [SerializeField]
    private ParticleSystem _effect;

    [SerializeField]
    private float _upgradeWaitTime = 0.5f;

    [SerializeField]
    private AudioSource _constructionSound;

    private int _state = 0;

    private void Start()
    {
        ChangeState();
    }

    private void OnEnable()
    {
        PlayerJoinNotifier.OnPlayerJoins += OnPlayerJoined;
    }

    private void OnDisable()
    {
        PlayerJoinNotifier.OnPlayerJoins -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == _playerSlot)
        {
            playerInput.gameObject.GetComponent<CoinObtainer>().OnCoinObtained += OnCoinObtained;
        }
    }

    private void OnCoinObtained(int[] obj, CoinBehaviour.MaterialType material)
    {
        var maxPerCoin = CountSystem.Instance.MaxPerCoin;
        if (obj[(int)material] == maxPerCoin[(int)material])
        {
            _state++;
            ChangeState();
        }
    }

    private async void ChangeState()
    {
        if (_state != 0)
        {
            _effect.Play();
            await Task.Delay((int)(_upgradeWaitTime * 1000));
        }
        for(int i = 0; i < _baseSteps.Length; i++)
        {
            _baseSteps[i].SetActive(false);
        }
        if (_constructionSound != null) _constructionSound.Play();
        _baseSteps[_state].SetActive(true);
        _effect.Pause(true);
    }
}
