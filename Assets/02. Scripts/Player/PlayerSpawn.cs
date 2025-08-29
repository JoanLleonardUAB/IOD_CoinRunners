using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    [Tooltip("In which places will the players spawn?")]
    private Transform[] _spawnPositions;

    [SerializeField]
    [Tooltip("What are the model prefabs?")]
    private GameObject[] _characterPrefabs;

    [SerializeField]
    [Tooltip("What are the filling containers?")]
    private FillingContainer[] _fillingContainers;

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
        Instantiate(_characterPrefabs[playerInput.playerIndex], playerInput.transform);
        playerInput
            .gameObject.GetComponent<PlayerStartingPosition>()
            .SetStartingPosition(_spawnPositions[playerInput.playerIndex].position);
        CoinObtainer coinObtainer = playerInput.gameObject.GetComponent<CoinObtainer>();
        if (coinObtainer != null && _fillingContainers.Length > playerInput.playerIndex)
        {
            coinObtainer.OnCoinObtained += _fillingContainers[playerInput.playerIndex].AddSalsa;
        }
    }
}
