using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CountSystem : MonoBehaviour
{
    public static CountSystem Instance;

    [SerializeField]
    [Tooltip("The camera that will follow the winner")]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    [Tooltip("The game object that will activate when there is a winner")]
    private GameObject _winnerUIObject;
    [SerializeField]
    [Tooltip("The game object that will activate when there is a tie")]
    private GameObject _tieUIObject;
    [SerializeField]
    [Tooltip("The game object that will always activate to allow the player to exit or replay")]
    private GameObject _replayUI;
    [SerializeField]
    private int[] _maxPerCoin;

    public int[] MaxPerCoin => _maxPerCoin;


    private List<GameObject> _playerList = new List<GameObject>();

    private void Awake()
    {
        _winnerUIObject.SetActive(false);
        _tieUIObject.SetActive(false);
        _replayUI.SetActive(false);
        _virtualCamera.Priority = 0;
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerJoinNotifier.OnPlayerJoins += OnPlayerJoined;
        GameTimer.OnTimerEnded += OnTimerEnded;
    }

    private void OnDisable()
    {
        PlayerJoinNotifier.OnPlayerJoins -= OnPlayerJoined;
        GameTimer.OnTimerEnded -= OnTimerEnded;
        foreach(var player in _playerList)
        {
            player.GetComponent<CoinObtainer>().OnCoinObtained -= OnCoinObtained;
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        _playerList.Add(playerInput.gameObject);
        playerInput.gameObject.GetComponent<CoinObtainer>().OnCoinObtained += OnCoinObtained;
    }

    private void OnCoinObtained(int[] coins, CoinBehaviour.MaterialType _)
    {
        if (PlayerWon() != -1)
        {
            OnTimerEnded();
        }
    }

    private int PlayerWon()
    {
        for (int i = 0; i < _playerList.Count; ++i)
        {
            CoinObtainer co = _playerList[i].GetComponent<CoinObtainer>();
            if (co.Coins[0] >= 3 && co.Coins[1] >= 2 && co.Coins[2] >= 1)
            {
                return i;
            }
        }
        return -1;
    }

    private void OnTimerEnded()
    {
        var wonIndex = PlayerWon();
        
        GameObject current = _playerList[wonIndex];

        _virtualCamera.Follow = current.transform;
        _virtualCamera.LookAt = current.transform;
        _virtualCamera.Priority = 50;
        _winnerUIObject.SetActive(true);
        current.GetComponentInChildren<Animator>().SetTrigger("Victory");
        _replayUI.SetActive(true);
    }
}
