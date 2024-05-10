using TMPro;
using UnityEngine;

public class BatteryInfo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The text object that shows the batteries of the player")]
    private TextMeshProUGUI _batteryText;

    // To activate the UI, activate the UI game object and subscribe to the coin obtain
    public void Activate(GameObject playerObject)
    {
        gameObject.SetActive(true);
        BatteryObtainer batteryObtainer = playerObject.GetComponent<BatteryObtainer>();
        batteryObtainer.OnBatteryObtained+= OnBatteryObtained;
    }

    // When you obtain a coin, show how many coins you have now
    private void OnBatteryObtained(int batteries)
    {
        _batteryText.text = batteries.ToString();
    }
}
