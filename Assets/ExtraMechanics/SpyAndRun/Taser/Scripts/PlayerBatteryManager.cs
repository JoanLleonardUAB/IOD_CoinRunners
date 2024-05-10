using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBatteryManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The list of objects that contain the UI for each player")]
    private BatteryInfo[] _playerInfoPanels;

    private void OnEnable()
    {
        PlayerJoinNotifier.OnPlayerJoins += OnPlayerJoined;
    }

    private void OnDisable()
    {
        PlayerJoinNotifier.OnPlayerJoins -= OnPlayerJoined;
    }

    // When a player joins, we activate the UI
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        _playerInfoPanels[playerInput.playerIndex].Activate(playerInput.gameObject);
    }
}
