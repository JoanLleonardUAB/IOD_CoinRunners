using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinNotifier : MonoBehaviour
{
    public static Action<PlayerInput> OnPlayerJoins;

    [SerializeField]
    private List<GameObject> markers;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        OnPlayerJoins?.Invoke(playerInput);

        var coinObtainer = playerInput.GetComponent<CoinObtainer>();
        if (coinObtainer != null)
        {
            coinObtainer.SetScoreMarker(markers[playerInput.playerIndex].transform);
            coinObtainer.GenerateDeliveryOrder();
        }
    }
}
