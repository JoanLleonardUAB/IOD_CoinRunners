using UnityEngine;

public class TaserMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _duration = 2f;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _effectDuration = 2f;

    private void Start()
    {
        Destroy(gameObject, _duration);
        _rb.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        var movement = other.collider.GetComponent<Movement>();
        if (movement != null)
        {
            movement.GetTasered(_effectDuration);
        }
        Destroy(gameObject);
    }
}
