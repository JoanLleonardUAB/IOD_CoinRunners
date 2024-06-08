using UnityEngine;

public class MeteorActivation : MonoBehaviour
{
    [SerializeField]
    private CoinSpawnManager _meteorSpawnManager;

    private void OnEnable()
    {
        GameTimer.OnPreparationEnded += StartMeteors;
        GameTimer.OnTimerEnded += StopMeteors;
    }
    private void OnDisable()
    {
        GameTimer.OnPreparationEnded -= StartMeteors;
        GameTimer.OnTimerEnded -= StopMeteors;
    }

    private void StartMeteors()
    {
        _meteorSpawnManager.gameObject.SetActive(true);
    }

    private void StopMeteors()
    {
        _meteorSpawnManager.gameObject.SetActive(false);
    }
}
