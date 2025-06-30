using UnityEngine;

public class FillingContainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("El objeto que representa la salsa dentro del bote")]
    private Transform salsaTransform;

    [SerializeField]
    [Tooltip("Altura máxima de la salsa cuando el bote está lleno")]
    private float maxFillHeight = 1.0f;

    [SerializeField]
    [Tooltip("Offset en Y para ajustar el pivote de la salsa")]
    private float pivotYOffset = 0f;

    private int maxCoins = 10; // Puedes ajustar este valor o hacerlo configurable
    private int currentCoins = 0;

    public void AddSalsa(int coins)
    {
        currentCoins = coins;
        float fillAmount = Mathf.Clamp01((float)currentCoins / maxCoins);

        // Ajusta la escala Y
        Vector3 localScale = salsaTransform.localScale;
        localScale.y = fillAmount * maxFillHeight;
        salsaTransform.localScale = localScale;

        // Calcula la posición Y para que la base quede fija aunque el pivote esté en el centro
        Vector3 localPosition = salsaTransform.localPosition;
        localPosition.y = -maxFillHeight + localScale.y + pivotYOffset;
        salsaTransform.localPosition = localPosition;

        // Si se alcanza el máximo de monedas, termina el juego
        if (currentCoins >= maxCoins)
        {
            GameTimer.OnTimerEnded?.Invoke();
        }
    }
}
