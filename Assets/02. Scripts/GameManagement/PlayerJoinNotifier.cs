using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinNotifier : MonoBehaviour
{
    public static Action<PlayerInput> OnPlayerJoins;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        OnPlayerJoins?.Invoke(playerInput);

        var coinObtainer = playerInput.GetComponent<CoinObtainer>();
        if (coinObtainer != null)
        {
            coinObtainer.GenerateDeliveryOrder();
        }
    }
}
