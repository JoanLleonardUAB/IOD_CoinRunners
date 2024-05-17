using UnityEngine;

public class SickleHitReceiver : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private CoinObtainer _obtainer;

    public void GotHit(int amount, Vector3 force)
    {
        _obtainer.LoseCoins(amount);

        _rb.AddForce(force);
    }
}
