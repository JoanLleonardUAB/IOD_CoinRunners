using UnityEngine;

public class CoinMarker : MonoBehaviour
{
    public void SetHighlight(bool highlight)
    {
        // Example: scale up or change color when highlighted
        transform.localScale = highlight ? Vector3.one * 1.2f : Vector3.one;
    }
}
